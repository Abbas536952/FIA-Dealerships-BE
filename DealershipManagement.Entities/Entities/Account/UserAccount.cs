using DealershipManagement.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DealershipManagement.Entities.Account
{
    public class UserAccount : IdentityUser
    {
        public int? MediaId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ResetPasswordToken { get; set; }
        public string VerifyEmailToken { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastLogin { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public AccountStatus Status { get; set; }
        public ThemeType Theme { get; set; } = ThemeType.Light;
        public Customer.Customer Customer { get; set; }
        public List<UserRole> Roles { get; set; }
    }
}
