using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.Entities.Account
{
    public class UserRole : IdentityUserRole<string>
    {
        public UserAccount Account { get; set; }
        public SystemRole Role { get; set; }
    }
}
