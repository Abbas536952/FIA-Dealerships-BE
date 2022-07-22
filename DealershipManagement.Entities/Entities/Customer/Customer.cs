using DealershipManagement.Entities.Account;
using DealershipManagement.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Repository.Entities;

namespace DealershipManagement.Entities.Customer
{
    public class Customer : AuditableDeletableEntity
    {
        public int Id { get; set; }
        public string UserAccountId { get; set; }
        public int? AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CustomerType Type { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string PhoneNumber { get; set; }
        public Address.Address Address { get; set; }
        public UserAccount UserAccount { get; set; }
        public List<Vehicle.Vehicle> Vehicles { get; set; }
    }
}
