using DealershipManagement.DataTransferObjects.Services.Address;
using DealershipManagement.Entities.Enums;
using System.Collections.Generic;

namespace DealershipManagement.DataTransferObjects.Services.Customer
{
    public class AddCustomerRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string PhoneNumber { get; set; }
        public CustomerType Type { get; set; }
        public AddressDto Address { get; set; }
    }
}
