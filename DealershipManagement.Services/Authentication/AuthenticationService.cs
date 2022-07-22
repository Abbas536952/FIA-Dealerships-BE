using DealershipManagement.Common;
using DealershipManagement.Common.ExceptionHandling;
using DealershipManagement.Common.Helpers;
using DealershipManagement.Common.Utilities.Interfaces;
using DealershipManagement.DataAccess.Helpers;
using DealershipManagement.DataTransferObjects.ApplicationConstants;
using DealershipManagement.DataTransferObjects.Enums;
using DealershipManagement.DataTransferObjects.Services.Authentication;
using DealershipManagement.DataTransferObjects.Services.Notification;
using DealershipManagement.Entities.Account;
using DealershipManagement.Entities.Enums;
using DealershipManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Template.Repository.Events;

namespace DealershipManagement.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        IUserManagerAccessor userManager;
        private readonly AuditableRepository<UserSession> userSessionRepository;
        private readonly AuditableRepository<UserAccount> accountRepository;
        private readonly AuditableRepository<Entities.Customer.Customer> customerRepository;
        private readonly INotificationSenderService notificationSenderService;
        private readonly IWorkContext<UserAccount, string> workContext;
        private readonly IConfiguration configuration;

        public AuthenticationService(
            AuditableRepository<UserSession> userSessionRepository,
            AuditableRepository<UserAccount> accountRepository,
            IUserManagerAccessor userManager,
            INotificationSenderService notificationSenderService,
            IWorkContext<UserAccount, string> workContext,
            AuditableRepository<Entities.Customer.Customer> customerRepository,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.userSessionRepository = userSessionRepository;
            this.accountRepository = accountRepository;
            this.notificationSenderService = notificationSenderService;
            this.customerRepository = customerRepository;
            this.workContext = workContext;
            this.configuration = configuration;
        }

        void OnBeforeSave(object sender, BeforeSaveEventArgs args) => args.UserName = this.workContext?.CurrentUser?.UserName ?? "System";

        public async Task<LoginResponseDto> RegisterAsync(RegistrationRequestDto request)
        {
            var account = await this.accountRepository.GetAll()
                .FirstOrDefaultAsync(x => x.NormalizedEmail == request.Email.ToUpper());

            if (account != null)
                CustomErrors.AccountAlreadyExists.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            account = new UserAccount
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                Status = AccountStatus.Active,
                CreatedBy = string.Empty,
                UpdatedBy = string.Empty,
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                Theme = ThemeType.Light
            };

            var accountCreationResult = await this.userManager.CreateAsync(account, request.Password);

            if (!accountCreationResult.Succeeded)
                CustomErrors.AccountCreationFailed.ThrowCustomErrorException(HttpStatusCode.BadRequest, string.Join(", ", accountCreationResult.Errors.Select(d => d.Description)));

            await this.userManager.AddUserToRoleAsync(account, SystemRoles.Customer);

            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration[AuthenticationConstants.JwtSecret]);
            var identity = new ClaimsIdentity(new GenericIdentity(account.Email, "Token"), new[] { new Claim("ID", account.Id) });

            foreach (var item in account.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, item.Role.Name));
            }

            var expiryTimeSpan = TimeSpan.FromDays(Convert.ToInt32(configuration[AuthenticationConstants.AccessTokenExpiryTime]));
            var expiry = DateTime.UtcNow.Add(expiryTimeSpan);
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512),
                Subject = identity,
                Expires = expiry,
                NotBefore = DateTime.UtcNow
            });

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            var session = new UserSession
            {
                UserAccount = account,
                RememberMe = request.IsRemember,
                Token = token,
                ExpiryDate = request.IsRemember ? DateTime.UtcNow.AddYears(100) : expiry,
                CreatedBy = "System"
            };
            await userSessionRepository.InsertAsync(session);
            await userSessionRepository.SaveAsync();

            account.LastLogin = DateTime.UtcNow;

            await this.accountRepository.UpdateAsync(account);
            await this.accountRepository.SaveAsync();

            var customer = new Entities.Customer.Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PrimaryEmail = request.Email,
                Type = CustomerType.Online,
                PhoneNumber = request.PhoneNumber
            };

            await this.customerRepository.InsertAsync(customer);
            await this.customerRepository.SaveAsync();

            return new LoginResponseDto
            {
                UserId = account.Id,
                Token = token,
                UserName = account.UserName,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Roles = account.Roles.Select(x => x.Role.Name).ToList(),
                Theme = account.Theme
            };
        }

        public async Task<string> InviteAsync(InviteUserRequestDto request)
        {
            var account = await this.accountRepository.GetAll()
                .FirstOrDefaultAsync(x => x.NormalizedEmail == request.Email.ToUpper());

            if (account != null)
                CustomErrors.AccountAlreadyExists.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            account = new UserAccount
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                Status = AccountStatus.Inactive,
                CreatedBy = string.Empty,
                UpdatedBy = string.Empty,
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                Theme = ThemeType.Light
            };

            var accountCreationResult = await this.userManager.CreateAsync(account);

            if (!accountCreationResult.Succeeded)
                CustomErrors.AccountCreationFailed.ThrowCustomErrorException(HttpStatusCode.BadRequest, string.Join(", ", accountCreationResult.Errors.Select(d => d.Description)));

            await this.userManager.AddUserToRoleAsync(account, request.Role.ToString());

            await this.notificationSenderService.SendAccountActivationEmailAsync(new AccountActivationNotificationRequestDto
            {
                EmailAddress = request.Email,
                FirstName = request.FirstName,
                UserId = account.Id
            });

            await this.accountRepository.UpdateAsync(account);
            await this.accountRepository.SaveAsync();

            return account.Id;
        }

        public async Task SetPasswordAsync(SetPasswordRequestDto request)
        {
            var user = await this.accountRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Id == request.UserId && u.Status == AccountStatus.Inactive);

            if (user == null)
                CustomErrors.UserNotFound.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            var result = await userManager.AddPasswordAsync(user, request.Password);

            if (!result.Succeeded)
                CustomErrors.Forbidden.ThrowCustomErrorException(HttpStatusCode.Forbidden);

            user.Status = AccountStatus.Active;

            await this.accountRepository.UpdateAsync(user);
            await this.accountRepository.SaveAsync();
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await userManager.GetAccounts()
                .Include(x => x.Roles)
                    .ThenInclude(y => y.Role)
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
                CustomErrors.UserNotFound.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            if (user.Status != AccountStatus.Active)
                CustomErrors.UserNotActive.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            if (!await userManager.CheckPasswordAsync(user, request.Password))
                CustomErrors.IncorrectPassword.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration[AuthenticationConstants.JwtSecret]);
            var identity = new ClaimsIdentity(new GenericIdentity(user.Email, "Token"), new[] { new Claim("ID", user.Id) });

            foreach (var item in user.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, item.Role.Name));
            }

            var expiryTimeSpan = TimeSpan.FromDays(Convert.ToInt32(configuration[AuthenticationConstants.AccessTokenExpiryTime]));
            var expiry = DateTime.UtcNow.Add(expiryTimeSpan);
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512),
                Subject = identity,
                Expires = expiry,
                NotBefore = DateTime.UtcNow
            });

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            var userSessionsFromRepo = await this.userSessionRepository.GetAll()
                .Where(x => x.UserAccount.Id == user.Id && x.ExpiryDate >= DateTime.UtcNow).ToListAsync();

            foreach (var userSession in userSessionsFromRepo)
            {
                userSession.ExpiryDate = DateTime.UtcNow;
                await this.userSessionRepository.UpdateAsync(userSession);
            }

            await this.userSessionRepository.SaveAsync();

            var session = new UserSession
            {
                UserAccount = user,
                RememberMe = request.RememberMe,
                Token = token,
                ExpiryDate = request.RememberMe ? DateTime.UtcNow.AddYears(100) : expiry,
                CreatedBy = "System",
                IsActive = true
            };
            await userSessionRepository.InsertAsync(session);
            await userSessionRepository.SaveAsync();

            user.LastLogin = DateTime.UtcNow;

            await this.accountRepository.UpdateAsync(user);
            await this.accountRepository.SaveAsync();

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                Token = token,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = user.Roles.Select(x => x.Role.Name).ToList(),
                Theme = user.Theme
            };

            response.CustomerId = await this.customerRepository.GetAll()
                .Where(c => c.UserAccountId == user.Id)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return response;
        }

        public async Task<EmailVerificationDto> SendForgetPasswordEmailAsync(string email)
        {
            var user = await this.accountRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                CustomErrors.UserNotFound.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            var token = await ForgotPasswordTokenGenerator(user);
            token = token.Substring(0, 4);

            if (token != null)
            {
                await this.notificationSenderService.SendForgotPasswordEmailAsync(new ForgotPasswordNotificationRequestDto
                {
                    EmailAddress = user.Email,
                    FirstName = user.FirstName,
                    Token = token,
                    UserId = user.Id,
                    UserName = user.UserName
                });
                return new EmailVerificationDto { IsValid = true };
            }
            return new EmailVerificationDto { IsValid = false };
        }

        public async Task UpdatePasswordAsync(UpdatePasswordDto request)
        {
            var currentUser = workContext.CurrentUser;

            var result = await userManager.ChangePasswordAsync(currentUser, request.CurrentPassword, request.UpdatedPassword);

            if (!result.Succeeded)
                throw new CustomErrorException(403, HttpStatusCode.PreconditionFailed, 
                    $"Errors occurred while adding account. Errors: {string.Join(", ", result.Errors.Select(e => $"{e.Code}:{e.Description}"))}");

            await this.accountRepository.UpdateAsync(currentUser);
            await this.accountRepository.SaveAsync();
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto request)
        {
            var user = await this.accountRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
                CustomErrors.UserNotFound.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            var result = await userManager.ResetPasswordAsync(user, user.ResetPasswordToken, request.UpdatedPassword);

            if (!result.Succeeded)
                CustomErrors.PasswordNotSet.ThrowCustomErrorException(HttpStatusCode.BadRequest, string.Join(", ", result.Errors.Select(e => $"{e.Code}:{e.Description}")));

            return true;
        }

        public async Task LogoutAsync()
        {
            var sessionToken = workContext.Token;

            var session = await userSessionRepository.GetAll()
                .Include(x => x.UserAccount)
                .FirstOrDefaultAsync(x => x.Token == sessionToken);

            session.ExpiryDate = DateTime.UtcNow;

            await userSessionRepository.UpdateAsync(session);
            await userSessionRepository.SaveAsync();
        }

        async Task<string> ForgotPasswordTokenGenerator(UserAccount user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            user.ResetPasswordToken = token;

            await accountRepository.UpdateAsync(user);
            await accountRepository.SaveAsync();

            return token;
        }

        public async Task<LoginResponseDto> MeAsync()
        {
            var user = this.workContext.CurrentUser;

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Theme = user.Theme,
                Token = workContext.Token,
                Roles = user.Roles.Select(r => r.Role.Name).ToList()
            };

            response.CustomerId = await this.customerRepository.GetAll()
                .Where(c => c.UserAccountId == user.Id)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return response;
        }

        public async Task<UserProfileResponseDto> GetAccountDetailsAsync()
        {
            var currentUser = workContext.CurrentUser;

            var response = await this.accountRepository.GetAll()
                .Select(acc => new UserProfileResponseDto
                {
                    Id = acc.Id,
                    FirstName = acc.FirstName,
                    LastName = acc.LastName,
                    EmailAddress = acc.Email,
                    Status = acc.Status,
                    LastLogin = acc.LastLogin,
                    PhoneNumber = acc.PhoneNumber,
                    Theme = acc.Theme,
                    Role = acc.Roles.FirstOrDefault().Role.Name
                })
                .FirstOrDefaultAsync(acc => acc.Id == currentUser.Id);

            return response;
        }

        public async Task UpdateAccountDetailsAsync(string accountId, UserProfileRequestDto request)
        {
            var currentUser = workContext.CurrentUser;
            var isAdmin = currentUser.Roles.Any(r => r.Role.Name == SystemRoles.GlobalAdmin);

            var account = await this.accountRepository.GetAll()
                .Include(acc => acc.Roles)
                    .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(acc => acc.Id == accountId);

            if (account == null)
                CustomErrors.UserNotFound.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            if (currentUser.Id != accountId && !isAdmin)
                CustomErrors.Forbidden.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            account.FirstName = request.FirstName;
            account.LastName = request.LastName;
            account.Email = request.EmailAddress;
            account.PhoneNumber = request.PhoneNumber;
            account.Theme = request.Theme;

            if (isAdmin)
            {
                account.Status = request.Status;

                if (request.Role != RoleType.Invalid && !account.Roles.Any(r => r.Role.Name == request.Role.ToString()))
                {
                    await userManager.RemoveUserFromRoleAsync(account, account.Roles.FirstOrDefault().Role.Name);
                    await userManager.AddUserToRoleAsync(account, request.Role.ToString());
                }
            }

            await this.accountRepository.UpdateAsync(account);
            await this.accountRepository.SaveAsync();
        }
    }
}
