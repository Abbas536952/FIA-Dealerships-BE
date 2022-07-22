using System;
using System.Collections.Generic;
using System.Text;

namespace DealershipManagement.DataTransferObjects.Services.Vehicle
{
    public class InspectionReportResponseDto
    {
        public int Exterior { get; set; }
        public int Interior { get; set; }
        public int Transmission { get; set; }
        public int AC { get; set; }
        public int Engine { get; set; }
        public int Suspension { get; set; }
    }
}
