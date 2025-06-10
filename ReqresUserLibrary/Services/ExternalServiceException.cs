namespace ReqresUserLibrary.Services
{
    public class ExternalServiceException : Exception
    {
        public string? ResponseBody { get; }

        public ExternalServiceException(string message, string? responseBody = null)
            : base(message)
        {
            ResponseBody = responseBody;
        }

        public ExternalServiceException(string message, Exception inner)
            : base(message, inner) { }
    }
}
