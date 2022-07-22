using DealershipManagement.Common.ExceptionHandling;
using System;

namespace Common.ExceptionHandling.Helpers
{
    public static class ResponseHelper
    {
        public static HttpResponseModel<T> AsSuccess<T>(this T data) =>
            new HttpResponseModel<T>
            {
                Data = data,
                Response = new Response
                {
                    Message = "Success"
                }
            };

        public static HttpResponseModel<object> AsSuccess() => AsSuccess<object>(null);

        public static HttpErrorResponseModel<object> AsFailure(this CustomErrorException exception)
        {
            var responseModel = new HttpErrorResponseModel<object>
            {
                Response = new ErrorResponse
                {
                    Message = exception.Description,
                    ErrorCode = (int)exception.Error
                },
                Data = exception.ResponseData,
            };

            return responseModel;
        }
    }
}
