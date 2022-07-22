using Microsoft.AspNetCore.Mvc;

namespace DealershipManagement.WebAPI.Filters
{
    public static class StartupExtensions
    {
        public static void AddExceptionFilter(this MvcOptions config) =>
                config.Filters.Add(typeof(HandleExceptionAttribute));
    }
}
