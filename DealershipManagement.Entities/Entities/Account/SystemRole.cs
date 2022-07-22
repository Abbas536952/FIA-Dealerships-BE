using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.Entities.Account
{
    public class SystemRole : IdentityRole<string>
    {
        public SystemRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public SystemRole(string roleName) : this()
        {
            this.Name = roleName;
        }

        public string Description { get; set; }
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public List<RoleClaim> Claims { get; set; } = new List<RoleClaim>();
        public bool IsActive { get; set; }
    }
}
