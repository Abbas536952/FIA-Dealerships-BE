namespace Common.ExceptionHandling
{
    public class HttpResponseModel<T>
    {
        /// <summary>Gets or sets the http status code.</summary>
        public Response Response { get; set; }

        /// <summary>Gets or sets the data.</summary>
        public T Data { get; set; }
    }
}
