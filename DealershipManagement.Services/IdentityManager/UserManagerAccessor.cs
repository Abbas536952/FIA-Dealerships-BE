using DealershipManagement.Entities.Account;
using DealershipManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace DealershipManagement.Services.IdentityManager
{
    public class UserManagerAccessor : IUserManagerAccessor
    {
        private readonly UserManager<UserAccount> userManager;

        /// <summary>
        /// Gets the stored users
        /// </summary>
        /// <returns>All users query</returns>
        public IQueryable<UserAccount> GetUsers() => this.userManager.Users;

        /// <summary>
        /// Initialize new instance of <param name="cref">UserManagerAccessor</param>
        /// </summary>
        /// <param name="userManager">User manager dependency</param>
        public UserManagerAccessor(UserManager<UserAccount> userManager) => this.userManager = userManager;

        /// <summary>
        /// Adds the password for the user
        /// </summary>
        /// <param name="existingAccount">Account on which password has to be set</param>
        /// <param name="password">Password to be set</param>
        public async Task<IdentityResult> AddPasswordAsync(UserAccount existingAccount, string password) => await userManager.AddPasswordAsync(existingAccount, password);

        /// <summary>
        /// Whether an account is in specified role or not
        /// </summary>
        /// <param name="account">Account on which the role is checked</param>
        /// <param name="role">Role to be chacked</param>
        /// <returns>Role exists in the account or not</returns>
        public async Task<bool> IsInRoleAsync(UserAccount account, string role)
        {
            var isInRole = await this.userManager.IsInRoleAsync(account, role);
            return isInRole;
        }

        /// <summary>
        /// Whether an account is in one of the specified roles or not
        /// </summary>
        /// <param name="account">Account on which the roles are checked</param>
        /// <param name="role">Roles to check</param>
        /// <returns>Roles exist in the account or not</returns>
        public async Task<bool> IsInAnyRoleAsync(UserAccount account, params string[] role)
        {
            for (int i = 0; i < role.Length; i++)
            {
                var result = await this.userManager.IsInRoleAsync(account, role[i]);
                if (result)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Whether an account is in all of the specified roles or not
        /// </summary>
        /// <param name="account">Account on which the roles are checked</param>
        /// <param name="role">Roles to check</param>
        /// <returns>Roles exist in the account or not</returns>
        /// <returns>Roles exist or not</returns>
        public async Task<bool> IsInAllRoleAsync(UserAccount account, params string[] role)
        {
            for (int i = 0; i < role.Length; i++)
            {
                var result = await this.userManager.IsInRoleAsync(account, role[i]);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// CHeck the password whether its correct for the specified accoint or not
        /// </summary>
        /// <param name="user">Account on which the password is checked</param>
        /// <param name="password">Password to be checked</param>
        /// <returns>Password is correct or not</returns>
        public async Task<bool> CheckPasswordAsync(UserAccount user, string password) => await userManager.CheckPasswordAsync(user, password);

        /// <summary>
        /// Verify email address based on the token sent to user email address
        /// </summary>
        /// <param name="existingAccount">A user account that exists in database</param>
        /// <param name="emailToken">The confirmation token sent to user email.</param>
        /// <returns>An <see cref="IdentityResult"/> after confirming email token.</returns>
        public async Task<IdentityResult> ConfirmEmailAsync(UserAccount existingAccount, string emailToken) => await this.userManager.ConfirmEmailAsync(existingAccount, emailToken);


        /// <summary>
        /// Create a new <see cref="UserAccount"/> using UserManager
        /// </summary>
        /// <param name="mappedAccount">New instance of <see cref="UserAccount"/> </param>
        /// <param name="requestModelPassword">Password for the newly created account.</param>
        /// <returns>An <see cref="IdentityResult"/> after creating user. </returns>
        public async Task<IdentityResult> CreateAsync(UserAccount mappedAccount, string requestModelPassword) => await this.userManager.CreateAsync(mappedAccount, requestModelPassword);

        /// <summary>
        /// Creates an account
        /// </summary>
        /// <param name="account">Account to be created</param>
        public async Task<IdentityResult> CreateAsync(UserAccount account) => await userManager.CreateAsync(account);

        /// <summary>
        /// Finds the account using email
        /// </summary>
        /// <param name="email">Using which an account is found</param>
        /// <returns>Found account</returns>
        public async Task<UserAccount> FindByEmailAsync(string email) => await userManager.FindByEmailAsync(email);

        /// <summary>
        /// Finds the account using id
        /// </summary>
        /// <param name="email">Using which an account is found</param>
        /// <returns>Found account</returns>
        public async Task<UserAccount> FindByIdAsync(string id) => await userManager.FindByIdAsync(id);

        /// <summary>
        /// Generate a random confirmation token for a user to verify it's email address.
        /// </summary>
        /// <param name="account">Existing user account</param>
        /// <returns>A string token against user email.</returns>
        public async Task<string> GenerateEmailConfirmationTokenAsync(UserAccount account) => await userManager.GenerateEmailConfirmationTokenAsync(account);

        /// <summary>
        /// Generates the reset password token
        /// </summary>
        /// <param name="user">Account againts the reset password token is generated</param>
        /// <returns>Reset password token</returns>
        public async Task<string> GeneratePasswordResetTokenAsync(UserAccount user) => await userManager.GeneratePasswordResetTokenAsync(user);

        /// <summary>
        /// Changes the password of the specified user
        /// </summary>
        /// <param name="user">User of which the password is changed</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password to be set</param>
        public async Task<IdentityResult> ChangePasswordAsync(UserAccount user, string oldPassword, string newPassword) => await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        /// <summary>
        /// Gets the list of users present
        /// </summary>
        /// <returns>List of users</returns>
        public IQueryable<UserAccount> GetAccounts() => userManager.Users;

        /// <summary>
        /// Resets the user password using the reset password token
        /// </summary>
        /// <param name="user">User of which the password is reset</param>
        /// <param name="tokenValue">Reset password token</param>
        /// <param name="updatedPassword">New password</param>
        /// <returns></returns>
        public async Task<IdentityResult> ResetPasswordAsync(UserAccount user, string tokenValue, string updatedPassword) => await userManager.ResetPasswordAsync(user, tokenValue, updatedPassword);

        /// <summary>
        /// Add user to specified role
        /// </summary>
        /// <param name="user">User to be added</param>
        /// <param name="role">Role that is assigned to that user</param>
        public async Task<IdentityResult> AddUserToRoleAsync(UserAccount user, string role) => await userManager.AddToRoleAsync(user, role);

        /// <summary>
        /// Removes the user from the specified role
        /// </summary>
        /// <param name="user">User to be removed</param>
        /// <param name="role">Role that is removed from that user</param>
        public async Task<IdentityResult> RemoveUserFromRoleAsync(UserAccount user, string role) => await userManager.RemoveFromRoleAsync(user, role);

        /// <summary>
        /// Verifies the reset token
        /// </summary>
        /// <param name="user">User on which the reset token is verified</param>
        /// <param name="token">Token to be verified</param>
        /// <returns>Verified or not</returns>
        public async Task<bool> VerifyResetTokenAsync(UserAccount user, string token)
        {
            if (await this.userManager.VerifyUserTokenAsync(user, this.userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
                return true;

            return false;
        }

        public async Task<bool> IsLockedOutAsync(UserAccount user) => await userManager.IsLockedOutAsync(user);
        public async Task AccessFailedAsync(UserAccount user) => await userManager.AccessFailedAsync(user);
        public async Task ResetAccessFailedCountAsync(UserAccount user) => await userManager.ResetAccessFailedCountAsync(user);
        public async Task SetLockoutEndDateAsync(UserAccount user, System.DateTimeOffset endDate) => await userManager.SetLockoutEndDateAsync(user, endDate);

        public async Task<IdentityResult> SetEmailAsync(UserAccount user, string email) => await this.userManager.SetEmailAsync(user, email);

        public async Task<string> GenerateChangeEmailTokenAsync(UserAccount user, string newEmail) =>
            await this.userManager.GenerateChangeEmailTokenAsync(user, newEmail);

        public async Task<IdentityResult> ChangeEmailAsync(UserAccount user, string newEmail, string token) =>
            await this.userManager.ChangeEmailAsync(user, newEmail, token);
    }
}
