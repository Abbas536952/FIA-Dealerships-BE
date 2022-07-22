using DealershipManagement.DataTransferObjects.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponseDto> RegisterAsync(RegistrationRequestDto request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<EmailVerificationDto> SendForgetPasswordEmailAsync(string email);
        Task UpdatePasswordAsync(UpdatePasswordDto request);
        Task<bool> ResetPasswordAsync(ResetPasswordDto request);
        Task LogoutAsync();
        Task<LoginResponseDto> MeAsync();
        Task<string> InviteAsync(InviteUserRequestDto request);
        Task SetPasswordAsync(SetPasswordRequestDto request);
        Task<UserProfileResponseDto> GetAccountDetailsAsync();
        Task UpdateAccountDetailsAsync(string accountId, UserProfileRequestDto request);
    }
}
