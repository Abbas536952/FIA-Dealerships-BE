using DealershipManagement.Entities.Account;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Repository.Entities;

namespace DealershipManagement.Entities.Account
{
    public class UserSession : AuditableDeletableEntity
    {
        public int Id { get; set; }
        public string UserAccountId { get; set; }
        public bool? RememberMe { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
