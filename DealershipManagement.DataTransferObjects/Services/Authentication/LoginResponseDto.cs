using DealershipManagement.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Services.Authentication
{
    public class LoginResponseDto
    {
        public int? CustomerId { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ThemeType Theme { get; set; }
    }
}
