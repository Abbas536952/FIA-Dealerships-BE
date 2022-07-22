using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Services.Authentication
{
    public class UpdatePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string UpdatedPassword { get; set; }
    }
}
