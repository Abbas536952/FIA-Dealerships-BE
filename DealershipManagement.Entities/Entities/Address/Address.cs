using DealershipManagement.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Repository.Entities;

namespace DealershipManagement.Entities.Address
{
    public class Address : AuditableDeletableEntity
    {
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public AddressType Type { get; set; }
    }
}
