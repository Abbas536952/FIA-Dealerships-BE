using DealershipManagement.DataTransferObjects.Common;
using DealershipManagement.DataTransferObjects.Services.Customer;
using DealershipManagement.DataTransferObjects.Vehicle;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Interfaces
{
    public interface IVehicleService
    {
        Task AddAsync(AddVehicleRequestDto request);
        Task<CustomerVehicleResponseDto> GetAsync(int vehicleId);
        Task UpdateAsync(int vehicleId, UpdateVehicleRequestDto request);
        Task<List<CustomerVehicleResponseDto>> ListAsync(ListRequestDto request);
        Task AssignToCustomerAsync(int vehicleId, int customerId);
        Task<List<CustomerVehicleResponseDto>> ListMyVehiclesAsync(int customerId, ListRequestDto request);
    }
}
