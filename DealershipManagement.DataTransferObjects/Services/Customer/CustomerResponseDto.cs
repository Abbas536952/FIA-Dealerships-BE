using DealershipManagement.DataTransferObjects.Services.Address;
using System;
using System.Collections.Generic;

namespace DealershipManagement.DataTransferObjects.Services.Customer
{
    public class CustomerResponseDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDto Address { get; set; }
        public List<CustomerVehicleResponseDto> Vehicles { get; set; }
    }
}
