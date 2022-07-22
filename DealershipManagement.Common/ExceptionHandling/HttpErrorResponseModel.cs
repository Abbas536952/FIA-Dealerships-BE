namespace Common.ExceptionHandling
{
    public class HttpErrorResponseModel<T>
    {
        /// <summary>Gets or sets the http status code.</summary>
        public ErrorResponse Response { get; set; }

        /// <summary>Gets or sets the data.</summary>
        public T Data { get; set; }
    }
}
