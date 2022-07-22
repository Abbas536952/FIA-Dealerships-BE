using DealershipManagement.Entities.Account;
using DealershipManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DealershipManagement.Services.IdentityManager
{
    /// <summary>
    /// Provides the common role management related features
    /// </summary>
    public class RoleManagerAccessor : IRoleManagerAccessor
    {
        private readonly RoleManager<SystemRole> roleManager;

        public RoleManagerAccessor(RoleManager<SystemRole> roleManager) => this.roleManager = roleManager;

        /// <summary>
        /// Whether an scpecified role exists or not
        /// </summary>
        /// <param name="role">Role name to check existence</param>
        /// <returns>Whether the role exists or not</returns>
        public async Task<bool> RoleExistsAsync(string role)
        {
            return await this.roleManager.RoleExistsAsync(role);
        }

        /// <summary>
        /// Creates a role in the system
        /// </summary>
        /// <param name="role">Role name to create</param>
        public async Task CreateAsync(SystemRole role)
        {
            await this.roleManager.CreateAsync(role);
        }

        /// <summary>
        /// Finds the role from its name
        /// </summary>
        /// <param name="roleName">Name to find the role</param>
        /// <returns>Found role</returns>
        public async Task<SystemRole> FindByNameAsync(string roleName)
        {
            return await this.roleManager.FindByNameAsync(roleName);
        }

        /// <summary>
        /// Finds the role from its id
        /// </summary>
        /// <param name="roleId">Id to find the role</param>
        /// <returns>Found role</returns>
        public async Task<SystemRole> FindByIdAsync(string roleId)
        {
            return await this.roleManager.FindByIdAsync(roleId);
        }

        /// <summary>
        /// Fetches the claims of the role specified
        /// </summary>
        /// <param name="role">Role of which claims are fetched</param>
        /// <returns>Role claims</returns>
        public async Task<IList<Claim>> GetClaimsAsync(SystemRole role)
        {
            return await this.roleManager.GetClaimsAsync(role);
        }

        /// <summary>
        /// Removes the claim specified form the specified role
        /// </summary>
        /// <param name="role">Role from which the claim is removed</param>
        /// <param name="claim">Claim to be removed</param>
        public async Task RemoveClaimAsync(SystemRole role, Claim claim)
        {
            await this.roleManager.RemoveClaimAsync(role, claim);
        }

        /// <summary>
        /// Deletes the role
        /// </summary>
        /// <param name="role">Role to be deleted</param>
        public async Task DeleteAsync(SystemRole role)
        {
            await this.DeleteAsync(role);
        }
    }
}
