using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Services.Notification
{
    public class AccountActivationNotificationRequestDto
    {
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string Template { get; set; }
    }
}
