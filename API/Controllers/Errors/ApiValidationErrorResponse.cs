using System.Net;

namespace API.Controllers.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(HttpStatusCode.BadRequest)
        {

        }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}