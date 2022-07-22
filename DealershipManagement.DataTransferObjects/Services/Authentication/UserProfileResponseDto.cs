using DealershipManagement.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Services.Authentication
{
    public class UserProfileResponseDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? LastLogin { get; set; }
        public AccountStatus Status { get; set; }
        public ThemeType Theme { get; set; }
        public string Role { get; set; }
    }
}
