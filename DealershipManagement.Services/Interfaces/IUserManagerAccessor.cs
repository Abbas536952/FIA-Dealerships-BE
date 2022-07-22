using DealershipManagement.Entities.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Interfaces
{
    public interface IUserManagerAccessor
    {
        /// <summary>
        /// Gets the stored users
        /// </summary>
        /// <returns>All users query</returns>
        IQueryable<UserAccount> GetUsers();

        /// <summary>
        /// Checks whether the UserAccount is in the specified role
        /// </summary>
        /// <param name="account"></param>
        /// <param name="role"></param>
        /// <returns>Whether the UserAccount is in specified role or not</returns>
        Task<bool> IsInRoleAsync(UserAccount account, string role);

        /// <summary>
        /// Whether an UserAccount is in one of the specified roles or not
        /// </summary>
        /// <param name="account">UserAccount on which the roles are checked</param>
        /// <param name="role">Roles to check</param>
        /// <returns>Roles exist in the UserAccount or not</returns>
        Task<bool> IsInAnyRoleAsync(UserAccount account, params string[] role);

        /// <summary>
        /// Whether an UserAccount is in all of the specified roles or not
        /// </summary>
        /// <param name="account">UserAccount on which the roles are checked</param>
        /// <param name="role">Roles to check</param>
        /// <returns>Roles exist in the UserAccount or not</returns>
        /// <returns>Roles exist or not</returns>
        Task<bool> IsInAllRoleAsync(UserAccount account, params string[] role);

        /// <summary>
        /// Generate a random confirmation token for a user to verify it's email address.
        /// </summary>
        /// <param name="account">Existing user account</param>
        /// <returns>A string token against user email.</returns>
        Task<string> GenerateEmailConfirmationTokenAsync(UserAccount account);

        /// <summary>
        /// Create a new <see cref="Account"/> using UserManager
        /// </summary>
        /// <param name="mappedAccount">New instance of <see cref="Account"/> </param>
        /// <param name="requestModelPassword">Password for the newly created account.</param>
        /// <returns>An <see cref="IdentityResult"/> after creating user. </returns>
        Task<IdentityResult> CreateAsync(UserAccount mappedAccount, string requestModelPassword);

        /// <summary>
        /// Verify email address based on the token sent to user email address
        /// </summary>
        /// <param name="existingAccount">A user UserAccount that exists in database</param>
        /// <param name="emailToken">The confirmation token sent to user email.</param>
        /// <returns>An <see cref="IdentityResult"/> after confirming email token.</returns>
        Task<IdentityResult> ConfirmEmailAsync(UserAccount existingAccount, string emailToken);

        IQueryable<UserAccount> GetAccounts();

        /// <summary>
        /// CHeck the password whether its correct for the specified accoint or not
        /// </summary>
        /// <param name="user">UserAccount on which the password is checked</param>
        /// <param name="password">Password to be checked</param>
        /// <returns>Password is correct or not</returns>
        Task<bool> CheckPasswordAsync(UserAccount user, string password);

        /// <summary>
        /// Finds the UserAccount using email
        /// </summary>
        /// <param name="email">Using which an UserAccount is found</param>
        /// <returns>Found account</returns>
        Task<UserAccount> FindByEmailAsync(string email);

        /// <summary>
        /// Generates the reset password token
        /// </summary>
        /// <param name="user">UserAccount againts the reset password token is generated</param>
        /// <returns>Reset password token</returns>
        Task<string> GeneratePasswordResetTokenAsync(UserAccount user);

        /// <summary>
        /// Changes the password of the specified user
        /// </summary>
        /// <param name="user">User of which the password is changed</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password to be set</param>
        Task<IdentityResult> ChangePasswordAsync(UserAccount user, string oldPassword, string newPassword);

        /// <summary>
        /// Finds the UserAccount using id
        /// </summary>
        /// <param name="email">Using which an UserAccount is found</param>
        /// <returns>Found account</returns>
        Task<UserAccount> FindByIdAsync(string id);

        /// <summary>
        /// Resets the user password using the reset password token
        /// </summary>
        /// <param name="user">User of which the password is reset</param>
        /// <param name="tokenValue">Reset password token</param>
        /// <param name="updatedPassword">New password</param>
        Task<IdentityResult> ResetPasswordAsync(UserAccount user, string tokenValue, string updatedPassword);

        /// <summary>
        /// Creates an account
        /// </summary>
        /// <param name="account">UserAccount to be created</param>
        Task<IdentityResult> CreateAsync(UserAccount account);

        /// <summary>
        /// Adds the password for the user
        /// </summary>
        /// <param name="existingAccount">UserAccount on which password has to be set</param>
        /// <param name="password">Password to be set</param>
        Task<IdentityResult> AddPasswordAsync(UserAccount existingAccount, string password);

        /// <summary>
        /// Add user to specified role
        /// </summary>
        /// <param name="user">User to be added</param>
        /// <param name="role">Role that is assigned to that user</param>
        Task<IdentityResult> AddUserToRoleAsync(UserAccount user, string role);

        /// <summary>
        /// Removes the user from the specified role
        /// </summary>
        /// <param name="user">User to be removed</param>
        /// <param name="role">Role that is removed from that user</param>
        Task<IdentityResult> RemoveUserFromRoleAsync(UserAccount user, string role);

        /// <summary>
        /// Verifies the reset token
        /// </summary>
        /// <param name="user">User on which the reset token is verified</param>
        /// <param name="token">Token to be verified</param>
        /// <returns>Verified or not</returns>
        Task<bool> VerifyResetTokenAsync(UserAccount user, string token);

        Task<IdentityResult> SetEmailAsync(UserAccount user, string email);

        Task<string> GenerateChangeEmailTokenAsync(UserAccount user, string newEmail);

        Task<IdentityResult> ChangeEmailAsync(UserAccount user, string newEmail, string token);
    }

}
