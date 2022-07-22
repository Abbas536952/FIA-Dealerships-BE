using DealershipManagement.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace DealershipManagement.DataTransferObjects.Vehicle
{
    public class UpdateVehicleRequestDto : VehicleResponseBaseDto
    {
        [EnumDataType(typeof(VehicleStatus))]
        public VehicleStatus Status { get; set; }
    }
}
