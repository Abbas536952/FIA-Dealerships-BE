using System.Collections.Generic;

namespace Common.ExceptionHandling
{
    public class ErrorResponse : Response
    {
        public int ErrorCode { get; set; }

        public List<string> Errors { get; set; }
    }
}
