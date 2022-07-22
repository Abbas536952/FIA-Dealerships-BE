using Common.ExceptionHandling;
using Common.ExceptionHandling.Helpers;
using DealershipManagement.DataTransferObjects.ApplicationConstants;
using DealershipManagement.DataTransferObjects.Common;
using DealershipManagement.DataTransferObjects.Services.Customer;
using DealershipManagement.Services.Interfaces;
using DealershipManagement.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealershipManagement.WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [CustomAuthorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpPost]
        [CustomAuthorize(SystemRoles.GlobalAdmin)]
        public async Task<HttpResponseModel<bool>> AddAsync(AddCustomerRequestDto request)
        {
            await this.customerService.AddAsync(request);
            return (true).AsSuccess();
        }

        [HttpPut("{id:int}")]
        [CustomAuthorize(SystemRoles.GlobalAdmin)]
        public async Task<HttpResponseModel<bool>> UpdateAsync(int id, UpdateCustomerRequestDto request)
        {
            await this.customerService.UpdateAsync(id, request);
            return (true).AsSuccess();
        }

        [HttpPost("list")]
        [CustomAuthorize(SystemRoles.GlobalAdmin)]
        public async Task<HttpResponseModel<List<CustomerListResponseDto>>> GetCustomerListAsync(ListRequestDto request)
        {
            var response = await this.customerService.GetCustomerListAsync(request);
            return (response).AsSuccess();
        }

        [HttpGet("{id:int}")]
        [CustomAuthorize(SystemRoles.GlobalAdmin)]
        public async Task<HttpResponseModel<CustomerResponseDto>> GetAsync(int id)
        {
            var response = await this.customerService.GetAsync(id);
            return (response).AsSuccess();
        }

        [HttpGet("{customerId:int}/transactions")]
        [CustomAuthorize(SystemRoles.GlobalAdmin)]
        public async Task<HttpResponseModel<List<TransactionResponseDto>>> ListTransactionsAsync(int customerId)
        {
            var response = await this.customerService.GetCustomerTransactionsAsync(customerId);
            return (response).AsSuccess();
        }

        [HttpPost("{customerId:int}/buy/{vehicleId:int}")]
        [CustomAuthorize(SystemRoles.Customer)]
        public async Task<HttpResponseModel<bool>> BuyVehicleAsync(int customerId, int vehicleId)
        {
            await this.customerService.BuyVehicleAsync(customerId, vehicleId);
            return (true).AsSuccess();
        }
    }
}
