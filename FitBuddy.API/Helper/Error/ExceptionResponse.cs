namespace ECommerce.API.Helper.Error
{
    public class ExceptionResponse : ApiResponse
    {
        public string? Details { get; set; }

        public ExceptionResponse(int statusCode, string? message = null, string? details = null)
            : base(statusCode, message)
        {
            Details = details;
        }
    }
}
