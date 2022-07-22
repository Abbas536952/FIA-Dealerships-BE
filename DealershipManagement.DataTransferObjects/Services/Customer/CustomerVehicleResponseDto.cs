using DealershipManagement.DataTransferObjects.Services.Vehicle;
using DealershipManagement.Entities.Enums;
using System;
using System.Collections.Generic;

namespace DealershipManagement.DataTransferObjects.Services.Customer
{
    public class CustomerVehicleResponseDto
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public decimal Cost { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public string ViewableLink { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }
        public decimal Mileage { get; set; }
        public VehicleStatus Status { get; set; }
        public DateTime? SalesDate { get; set; }
        public string Registration { get; set; }
        public string Description { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public InspectionReportResponseDto InspectionReport { get; set; }
    }
}
