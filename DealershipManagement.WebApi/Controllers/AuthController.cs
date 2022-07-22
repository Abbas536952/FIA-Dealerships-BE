using System.Threading.Tasks;
using Common.ExceptionHandling;
using Common.ExceptionHandling.Helpers;
using DealershipManagement.DataTransferObjects.ApplicationConstants;
using DealershipManagement.DataTransferObjects.Services.Authentication;
using DealershipManagement.Services.Interfaces;
using DealershipManagement.WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealershipManagement.WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService authService;

        public AuthController(IAuthenticationService authService) 
        { 
            this.authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<HttpResponseModel<LoginResponseDto>> RegisterAsync(RegistrationRequestDto request)
        {
            var response = await this.authService.RegisterAsync(request);
            return (response).AsSuccess();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<HttpResponseModel<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var response = await this.authService.LoginAsync(request);
            return (response).AsSuccess();
        }

        [HttpPost("forgetpassword")]
        [AllowAnonymous]
        public async Task<HttpResponseModel<ForgotPasswordResponseDto>> SendForgetPasswordEmailAsync(ForgotPasswordDto request)
        {
            var response = await this.authService.SendForgetPasswordEmailAsync(request.Email);
            return new ForgotPasswordResponseDto { IsValid = response.IsValid }.AsSuccess();
        }

        [HttpPost("updatepassword")]
        [CustomAuthorize]
        public async Task<object> UpdatePasswordAsync([FromBody]UpdatePasswordDto model)
        {
            await this.authService.UpdatePasswordAsync(model);
            return (true).AsSuccess();
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<HttpResponseModel<bool>> ResetPasswordAsync([FromBody]ResetPasswordDto model)
        {
            var response = await this.authService.ResetPasswordAsync(model);
            return (true).AsSuccess();
        }

        [HttpPost("logout")]
        [CustomAuthorize]
        public async Task<object> Logout()
        {
            await this.authService.LogoutAsync();
            return (true).AsSuccess();
        }

        [HttpGet("me")]
        [CustomAuthorize]
        public async Task<HttpResponseModel<LoginResponseDto>> MeAsync()
        {
            var result = await this.authService.MeAsync();
            return result.AsSuccess();
        }

        [HttpPost("invite")]
        [CustomAuthorize(SystemRoles.GlobalAdmin)]
        public async Task<HttpResponseModel<bool>> InviteAsync(InviteUserRequestDto request)
        {
            await this.authService.InviteAsync(request);
            return (true).AsSuccess();
        }

        [HttpPost("setpassword")]
        [AllowAnonymous]
        public async Task<HttpResponseModel<bool>> SetPasswordAsync(SetPasswordRequestDto request)
        {
            await this.authService.SetPasswordAsync(request);
            return (true).AsSuccess();
        }

        [HttpGet("myprofile")]
        [CustomAuthorize]
        public async Task<HttpResponseModel<UserProfileResponseDto>> GetAccountDetailsAsync()
        {
            var response = await this.authService.GetAccountDetailsAsync();
            return response.AsSuccess();
        }

        [HttpPut("profile/{accountId}")]
        [CustomAuthorize]
        public async Task<HttpResponseModel<bool>> UpdateAccountDetailsAsync(string accountId, UserProfileRequestDto request)
        {
            await this.authService.UpdateAccountDetailsAsync(accountId, request);
            return true.AsSuccess();
        }
    }
}