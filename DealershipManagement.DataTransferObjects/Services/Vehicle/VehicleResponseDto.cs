using DealershipManagement.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Services.Vehicle
{
    public class VehicleResponseDto
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public string ViewableLink { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }
        public decimal Mileage { get; set; }
        public DateTime? SalesDate { get; set; }
        public VehicleStatus Status { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public InspectionReportResponseDto InspectionReport { get; set; }
    }
}
