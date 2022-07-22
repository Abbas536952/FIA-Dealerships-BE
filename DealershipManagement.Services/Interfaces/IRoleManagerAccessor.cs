using DealershipManagement.Entities.Account;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Interfaces
{
    /// <summary>
    /// Provides the common role management related features
    /// </summary>
    public interface IRoleManagerAccessor
    {
        /// <summary>
        /// Whether an scpecified role exists or not
        /// </summary>
        /// <param name="role">Role name to check existence</param>
        /// <returns>Whether the role exists or not</returns>
        Task<bool> RoleExistsAsync(string role);

        /// <summary>
        /// Creates a role in the system
        /// </summary>
        /// <param name="role">Role name to create</param>
        Task CreateAsync(SystemRole role);

        /// <summary>
        /// Finds the role from its name
        /// </summary>
        /// <param name="roleName">Name to find the role</param>
        /// <returns>Found role</returns>
        Task<SystemRole> FindByNameAsync(string roleName);

        /// <summary>
        /// Finds the role from its id
        /// </summary>
        /// <param name="roleId">Id to find the role</param>
        /// <returns>Found role</returns>
        Task<SystemRole> FindByIdAsync(string roleId);

        /// <summary>
        /// Fetches the claims of the role specified
        /// </summary>
        /// <param name="role">Role of which claims are fetched</param>
        /// <returns>Role claims</returns>
        Task<IList<Claim>> GetClaimsAsync(SystemRole role);

        /// <summary>
        /// Removes the claim specified form the specified role
        /// </summary>
        /// <param name="role">Role from which the claim is removed</param>
        /// <param name="claim">Claim to be removed</param>
        Task RemoveClaimAsync(SystemRole role, Claim claim);

        /// <summary>
        /// Deletes the role
        /// </summary>
        /// <param name="role">Role to be deleted</param>
        Task DeleteAsync(SystemRole role);
    }
}
