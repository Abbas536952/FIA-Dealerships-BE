using DealershipManagement.DataTransferObjects.Enums;
using DealershipManagement.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Services.Authentication
{
    public class UserProfileRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public AccountStatus Status { get; set; }
        public ThemeType Theme { get; set; }
        public RoleType Role { get; set; }
    }
}
