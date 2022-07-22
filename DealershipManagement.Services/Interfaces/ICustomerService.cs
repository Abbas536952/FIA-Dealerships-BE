using DealershipManagement.DataTransferObjects.Common;
using DealershipManagement.DataTransferObjects.Services.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DealershipManagement.Services.Interfaces
{
    public interface ICustomerService
    {
        Task AddAsync(AddCustomerRequestDto request);
        Task<List<CustomerListResponseDto>> GetCustomerListAsync(ListRequestDto request);
        Task UpdateAsync(int customerId, UpdateCustomerRequestDto request);
        Task<CustomerResponseDto> GetAsync(int customerId);
        Task BuyVehicleAsync(int customerId, int vehicleId);
        Task<List<TransactionResponseDto>> GetCustomerTransactionsAsync(int customerId);
    }
}
