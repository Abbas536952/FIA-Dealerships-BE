using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DealershipManagement.Common.ExceptionHandling
{
    public class CustomErrorException : Exception
    {
        public CustomErrorException(
            int error = 0,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            string description = null,
            object data = null)
        {
            this.Error = error;
            this.StatusCode = statusCode;
            this.Description = description;
            this.ResponseData = data;
        }

        /// <summary>Gets the error.</summary>
        public int Error { get; }

        /// <summary>Gets the status code.</summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>Gets the description.</summary>
        public string Description { get; }

        /// <summary>Gets the data.</summary>
        public object ResponseData { get; }

        public override string Message => Description;
    }
}
