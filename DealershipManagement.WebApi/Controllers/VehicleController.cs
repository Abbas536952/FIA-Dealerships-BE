using Common.ExceptionHandling;
using Common.ExceptionHandling.Helpers;
using DealershipManagement.DataTransferObjects.ApplicationConstants;
using DealershipManagement.DataTransferObjects.Common;
using DealershipManagement.DataTransferObjects.Services.Customer;
using DealershipManagement.DataTransferObjects.Vehicle;
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
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [HttpGet("{vehicleId:int}")]
        public async Task<HttpResponseModel<CustomerVehicleResponseDto>> GetVehicleAsync(int vehicleId)
        {
            var response = await this.vehicleService.GetAsync(vehicleId);
            return (response).AsSuccess();
        }

        [HttpPost]
        [CustomAuthorize(SystemRoles.GlobalAdmin)]
        public async Task<HttpResponseModel<bool>> AddVehicleAsync(AddVehicleRequestDto request)
        {
            await this.vehicleService.AddAsync(request);
            return (true).AsSuccess();
        }

        [HttpPut("{vehicleId:int}")]
        [CustomAuthorize(SystemRoles.GlobalAdmin)]
        public async Task<HttpResponseModel<bool>> UpdateVehicleAsync(int vehicleId, UpdateVehicleRequestDto request)
        {
            await this.vehicleService.UpdateAsync(vehicleId, request);
            return (true).AsSuccess();
        }

        [HttpPost("{vehicleId:int}/assign-to/{customerId:int}")]
        [CustomAuthorize(SystemRoles.GlobalAdmin)]
        public async Task<HttpResponseModel<bool>> AssignToCustomerAsync(int vehicleId, int customerId)
        {
            await this.vehicleService.AssignToCustomerAsync(vehicleId, customerId);
            return (true).AsSuccess();
        }

        [HttpPost("list")]
        public async Task<HttpResponseModel<List<CustomerVehicleResponseDto>>> ListVehiclesAsync(ListRequestDto request)
        {
            var response = await this.vehicleService.ListAsync(request);
            return (response).AsSuccess();
        }

        [HttpPost("{customerId:int}/vehicles")]
        public async Task<HttpResponseModel<List<CustomerVehicleResponseDto>>> ListMyVehiclesAsync(int customerId, ListRequestDto request)
        {
            var response = await this.vehicleService.ListMyVehiclesAsync(customerId, request);
            return (response).AsSuccess();
        }
    }
}
