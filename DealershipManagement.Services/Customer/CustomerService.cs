using DealershipManagement.Common;
using DealershipManagement.Common.Helpers;
using DealershipManagement.Common.Utilities.Interfaces;
using DealershipManagement.DataAccess.Helpers;
using DealershipManagement.DataTransferObjects.Common;
using DealershipManagement.DataTransferObjects.Enums;
using DealershipManagement.DataTransferObjects.Services.Address;
using DealershipManagement.DataTransferObjects.Services.Authentication;
using DealershipManagement.DataTransferObjects.Services.Customer;
using DealershipManagement.DataTransferObjects.Services.Vehicle;
using DealershipManagement.Entities.Account;
using DealershipManagement.Entities.Enums;
using DealershipManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly AuditableRepository<Entities.Customer.Customer> customerRepository;
        private readonly AuditableRepository<Entities.Vehicle.Vehicle> vehicleRepository;
        private readonly AuditableRepository<Entities.Customer.Transaction> transactionRepository;
        private readonly IWorkContext<UserAccount, string> workContext;
        private readonly IAuthenticationService authenticationService;

        public CustomerService(
            AuditableRepository<Entities.Customer.Customer> customerRepository,
            AuditableRepository<Entities.Vehicle.Vehicle> vehicleRepository,
            IWorkContext<UserAccount, string> workContext,
            AuditableRepository<Entities.Customer.Transaction> transactionRepository,
            IAuthenticationService authenticationService)
        {
            this.customerRepository = customerRepository;
            this.vehicleRepository = vehicleRepository;
            this.transactionRepository = transactionRepository;
            this.workContext = workContext;
            this.authenticationService = authenticationService;
        }

        public async Task AddAsync(AddCustomerRequestDto request)
        {
            var customer = await this.customerRepository.GetAll()
                .FirstOrDefaultAsync(x => 
                        (!string.IsNullOrEmpty(request.PrimaryEmail) && x.PrimaryEmail == request.PrimaryEmail) ||
                        (!string.IsNullOrEmpty(request.SecondaryEmail) && x.SecondaryEmail == request.SecondaryEmail));

            if (customer != null)
                CustomErrors.CustomerAlreadyExists.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            var customerToInsert = new Entities.Customer.Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PrimaryEmail = request.PrimaryEmail,
                SecondaryEmail = request.SecondaryEmail,
                Type = request.Type,
                PhoneNumber = request.PhoneNumber
            };

            if (request.Address != null)
                customerToInsert.Address = new Entities.Address.Address
                {
                    StreetAddress = request.Address.StreetAddress,
                    ZipCode = request.Address.ZipCode,
                    State = request.Address.State,
                    City = request.Address.City,
                    Country = request.Address.Country,
                    Type = request.Address.Type
                };

            var invitationRequest = new InviteUserRequestDto
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.PrimaryEmail,
                PhoneNumber = request.PhoneNumber,
                Role = RoleType.Customer
            };
            var accountId = await this.authenticationService.InviteAsync(invitationRequest);

            customerToInsert.UserAccountId = accountId;

            await this.customerRepository.InsertAsync(customerToInsert);
            await this.customerRepository.SaveAsync();
        }

        public async Task<CustomerResponseDto> GetAsync(int customerId)
        {
            var customer = await this.customerRepository.GetAll()
                .Select(c => new CustomerResponseDto
                {
                    CustomerId = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PrimaryEmail = c.PrimaryEmail,
                    SecondaryEmail = c.SecondaryEmail,
                    PhoneNumber = c.PhoneNumber,
                    Address = new AddressDto
                    {
                        StreetAddress = c.Address.StreetAddress,
                        State = c.Address.State,
                        City = c.Address.City,
                        Country = c.Address.Country,
                        Type = c.Address.Type,
                        ZipCode = c.Address.ZipCode
                    },
                    Vehicles = c.Vehicles
                    .Select(v => new CustomerVehicleResponseDto
                    {
                        Id = v.Id,
                        CustomerId = v.CustomerId,
                        Make = v.Make,
                        Model = v.Model,
                        Year = v.Year,
                        Mileage = v.Mileage,
                        Status = v.Status,
                        VIN = v.VIN,
                        ViewableLink = v.ViewableLink,
                        SalesDate = v.SalesDate,
                        TransmissionType = v.TransmissionType,
                        Cost = v.Cost,
                        Description = v.Description,
                        Registration = v.Registration,
                        InspectionReport = new InspectionReportResponseDto
                        {
                            AC = v.ACReport,
                            Suspension = v.SuspensionReport,
                            Engine = v.EngineReport,
                            Exterior = v.ExteriorReport,
                            Interior = v.InteriorReport,
                            Transmission = v.TransmissionReport
                        }
                    })
                    .ToList(),
                })
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
                CustomErrors.CustomerNotFound.ThrowCustomErrorException(HttpStatusCode.NotFound);

            return customer;
        }

        public async Task UpdateAsync(int customerId, UpdateCustomerRequestDto request)
        {
            var customer = await this.customerRepository.GetAll()
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                CustomErrors.CustomerNotFound.ThrowCustomErrorException(HttpStatusCode.NotFound);

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.PrimaryEmail = request.PrimaryEmail;
            customer.SecondaryEmail = request.SecondaryEmail;
            customer.PhoneNumber = request.PhoneNumber;
            
            if (request.Address != null)
            {
                customer.Address = new Entities.Address.Address
                {
                    StreetAddress = request.Address.StreetAddress,
                    City = request.Address.City,
                    State = request.Address.State,
                    Country = request.Address.Country,
                    ZipCode = request.Address.ZipCode,
                    Type = request.Address.Type
                };
            }

            await this.customerRepository.UpdateAsync(customer);
            await this.customerRepository.SaveAsync();
        }

        public async Task<List<CustomerListResponseDto>> GetCustomerListAsync(ListRequestDto request)
        {
            var response = await this.customerRepository.GetAll()
                .Where(c => string.IsNullOrEmpty(request.SearchBy) || c.PhoneNumber.Contains(request.SearchBy)
                    || c.PrimaryEmail.Contains(request.SearchBy) || c.SecondaryEmail.Contains(request.SearchBy) || c.FirstName.Contains(request.SearchBy)
                    || c.LastName.Contains(request.SearchBy))
                .Select(c => new CustomerListResponseDto
                {
                    CustomerId = c.Id,
                    PhoneNumber = c.PhoneNumber,
                    PrimaryEmail = c.PrimaryEmail,
                    SecondaryEmail = c.SecondaryEmail,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Address = new AddressDto
                    {
                        StreetAddress = c.Address.StreetAddress,
                        City = c.Address.City,
                        Country = c.Address.Country,
                        State = c.Address.State,
                        Type = c.Address.Type,
                        ZipCode = c.Address.ZipCode
                    },
                    Vehicles = c.Vehicles
                    .Select(v => new VehicleResponseDto
                    {
                        Id = v.Id,
                        Make = v.Make,
                        Model = v.Model,
                        Year = v.Year,
                        Mileage = v.Mileage,
                        Status = v.Status,
                        VIN = v.VIN,
                        ViewableLink = v.ViewableLink,
                        SalesDate = v.SalesDate,
                        TransmissionType = v.TransmissionType,
                        InspectionReport = new InspectionReportResponseDto
                        {
                            AC = v.ACReport,
                            Suspension = v.SuspensionReport,
                            Engine = v.EngineReport,
                            Exterior = v.ExteriorReport,
                            Interior = v.InteriorReport,
                            Transmission = v.TransmissionReport
                        }
                    })
                    .ToList()
                })
                .Skip(request.Offset)
                .ToListAsync();

            return response;
        }

        public async Task<List<TransactionResponseDto>> GetCustomerTransactionsAsync(int customerId)
        {
            var response = await this.transactionRepository.GetAll()
                .Where(t => t.CustomerId == customerId)
                .Select(t => new TransactionResponseDto
                {
                    Id = t.Id,
                    CustomerId = t.CustomerId,
                    VehicleId = t.VehicleId,
                    CustomerName = t.Customer.FirstName + " " + t.Customer.LastName,
                    VehicleDetails = t.Vehicle.Year + " " + t.Vehicle.Make + " " + t.Vehicle.Model,
                    Date = t.CreatedOn,
                    TotalCost = t.TotalCost
                })
                .ToListAsync();

            return response;
        }

        public async Task BuyVehicleAsync(int customerId, int vehicleId)
        {
            if (!await this.customerRepository.GetAll().AnyAsync(c => c.Id == customerId))
                CustomErrors.CustomerNotFound.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            var vehicle = await this.vehicleRepository.GetAll()
                .FirstOrDefaultAsync(v => v.Id == vehicleId && v.Status == VehicleStatus.Active);

            if (vehicle == null)
                CustomErrors.VehicleNotFound.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            vehicle.CustomerId = customerId;
            vehicle.Status = VehicleStatus.Owned;
            vehicle.SalesDate = DateTime.UtcNow;

            await this.vehicleRepository.UpdateAsync(vehicle);
            await this.vehicleRepository.SaveAsync();

            var transaction = new Entities.Customer.Transaction
            {
                CustomerId = customerId,
                VehicleId = vehicleId,
                TotalCost = vehicle.Cost
            };
            await this.transactionRepository.InsertAsync(transaction);
            await this.transactionRepository.SaveAsync();
        }
    }
}
