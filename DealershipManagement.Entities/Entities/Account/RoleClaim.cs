using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.Entities.Account
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public bool IsUpdatable { get; set; }
        public int ClaimId { get; set; }
        public SystemRole Role { get; set; }
        public SystemClaim Claim { get; set; }
    }
}
