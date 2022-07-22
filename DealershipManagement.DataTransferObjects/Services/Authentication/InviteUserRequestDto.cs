using DealershipManagement.DataTransferObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Services.Authentication
{
    public class InviteUserRequestDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [EnumDataType(typeof(RoleType))]
        public RoleType Role { get; set; }
    }
}
