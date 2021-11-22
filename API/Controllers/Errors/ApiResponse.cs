using System.Net;

namespace API.Controllers.Errors
{
    public class ApiResponse
    {
        public ApiResponse(HttpStatusCode statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => "A bad request, you have made",
                HttpStatusCode.Unauthorized => "Not authorizedAuthorized, you are not",
                HttpStatusCode.NotFound => "Resource found, it was not",
                HttpStatusCode.InternalServerError => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hats. Hate leads to career change",
                _ => null
            };
        }

    }
}