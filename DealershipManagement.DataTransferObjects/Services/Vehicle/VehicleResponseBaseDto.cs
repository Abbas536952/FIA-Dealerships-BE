using DealershipManagement.DataTransferObjects.Services.Vehicle;
using DealershipManagement.Entities.Enums;
using System.Collections.Generic;

namespace DealershipManagement.DataTransferObjects.Vehicle
{
    public class VehicleResponseBaseDto
    {
        public string Model { get; set; }
        public string VIN { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }
        public decimal Mileage { get; set; }
        public string ViewableLink { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string Registration { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public InspectionReportResponseDto InspectionReport { get; set; }
    }
}
