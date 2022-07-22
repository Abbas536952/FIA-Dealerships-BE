using DealershipManagement.Common;
using DealershipManagement.Common.Helpers;
using DealershipManagement.DataAccess.Helpers;
using DealershipManagement.DataTransferObjects.Common;
using DealershipManagement.DataTransferObjects.Services.Customer;
using DealershipManagement.DataTransferObjects.Services.Vehicle;
using DealershipManagement.DataTransferObjects.Vehicle;
using DealershipManagement.Entities.Enums;
using DealershipManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Vehicle
{
    public class VehicleService : IVehicleService
    {
        private readonly AuditableRepository<Entities.Customer.Customer> customerRepository;
        private readonly AuditableRepository<Entities.Vehicle.Vehicle> vehicleRepository;

        public VehicleService(
            AuditableRepository<Entities.Customer.Customer> customerRepository,
            AuditableRepository<Entities.Vehicle.Vehicle> vehicleRepository)
        {
            this.customerRepository = customerRepository;
            this.vehicleRepository = vehicleRepository;
        }

        public async Task AddAsync(AddVehicleRequestDto request)
        {
            if (await this.vehicleRepository.GetAll().AnyAsync(v => v.VIN == request.VIN))
                CustomErrors.VINAlreadyExists.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            var vehicle = new Entities.Vehicle.Vehicle
            {
                Make = request.Make,
                Mileage = request.Mileage,
                Status = VehicleStatus.Active,
                Model = request.Model,
                VIN = request.VIN,
                Year = request.Year,
                ViewableLink = request.ViewableLink,
                Cost = request.Cost,
                Registration = request.Registration,
                Description = request.Description,
                TransmissionType = request.TransmissionType,
                ExteriorReport = request.InspectionReport.Exterior,
                InteriorReport = request.InspectionReport.Interior,
                SuspensionReport = request.InspectionReport.Suspension,
                ACReport = request.InspectionReport.AC,
                EngineReport = request.InspectionReport.Engine,
                TransmissionReport = request.InspectionReport.Transmission
            };

            await this.vehicleRepository.InsertAsync(vehicle);
            await this.vehicleRepository.SaveAsync();
        }

        public async Task UpdateAsync(int vehicleId, UpdateVehicleRequestDto request)
        {
            if (await this.vehicleRepository.GetAll().AnyAsync(v => v.VIN == request.VIN && v.Id != vehicleId))
                CustomErrors.VINAlreadyExists.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            var vehicle = await this.vehicleRepository.GetAll()
                .FirstOrDefaultAsync(v => v.Id == vehicleId);

            if (vehicle == null)
                CustomErrors.VehicleNotFound.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            vehicle.Status = request.Status;
            vehicle.Make = request.Make;
            vehicle.Mileage = request.Mileage;
            vehicle.Model = request.Model;
            vehicle.VIN = request.VIN;
            vehicle.Year = request.Year;
            vehicle.ViewableLink = request.ViewableLink;
            vehicle.Cost = request.Cost;
            vehicle.Registration = request.Registration;
            vehicle.Description = request.Description;
            vehicle.TransmissionType = request.TransmissionType;
            vehicle.ExteriorReport = request.InspectionReport.Exterior;
            vehicle.InteriorReport = request.InspectionReport.Interior;
            vehicle.SuspensionReport = request.InspectionReport.Suspension;
            vehicle.ACReport = request.InspectionReport.AC;
            vehicle.EngineReport = request.InspectionReport.Engine;
            vehicle.TransmissionReport = request.InspectionReport.Transmission;

            await this.vehicleRepository.UpdateAsync(vehicle);
            await this.vehicleRepository.SaveAsync();
        }

        public async Task<CustomerVehicleResponseDto> GetAsync(int vehicleId)
        {
            var vehicle = await this.vehicleRepository.GetAll()
                .Where(v => v.Id == vehicleId)
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
                    Cost = v.Cost,
                    Registration = v.Registration,
                    Description = v.Description,
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
                .FirstOrDefaultAsync();

            if (vehicle == null)
                CustomErrors.VehicleNotFound.ThrowCustomErrorException(HttpStatusCode.BadRequest);

            return vehicle;
        }

        public async Task<List<CustomerVehicleResponseDto>> ListAsync(ListRequestDto request)
        {
            var response = await this.vehicleRepository.GetAll()
                .Where(v => v.Status != VehicleStatus.Owned && (string.IsNullOrEmpty(request.SearchBy) || v.Make.Contains(request.SearchBy)
                    || v.Model.Contains(request.SearchBy) || v.VIN.Contains(request.SearchBy) || v.Registration.Contains(request.SearchBy)
                    || v.Description.Contains(request.SearchBy)))
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
                    Cost = v.Cost,
                    Registration = v.Registration,
                    Description = v.Description,
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
                .Skip(request.Offset)
                .ToListAsync();

            return response;
        }

        public async Task<List<CustomerVehicleResponseDto>> ListMyVehiclesAsync(int customerId, ListRequestDto request)
        {
            var response = await this.vehicleRepository.GetAll()
                .Where(v => v.CustomerId == customerId && (string.IsNullOrEmpty(request.SearchBy) || v.Make.Contains(request.SearchBy)
                    || v.Model.Contains(request.SearchBy) || v.VIN.Contains(request.SearchBy) || v.Registration.Contains(request.SearchBy)
                    || v.Description.Contains(request.SearchBy)))
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
                    Cost = v.Cost,
                    Registration = v.Registration,
                    Description = v.Description,
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
                .Skip(request.Offset)
                .ToListAsync();

            return response;
        }

        public async Task AssignToCustomerAsync(int vehicleId, int customerId)
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
        }
    }
}
