using System.Threading.Tasks;
using Common.ExceptionHandling.Helpers;
using DealershipManagement.Common.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DealershipManagement.WebAPI.Filters
{
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            if(context.Exception is CustomErrorException exception)
            {
                var exceptionResult = exception.AsFailure();
                context.HttpContext.Response.StatusCode = (int)exception.StatusCode;
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = new ObjectResult(exceptionResult);
            }

            await base.OnExceptionAsync(context);
        }
    }
}
