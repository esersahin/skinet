using System.Net;
using API.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("errors/{code}")]
    public class ErrorController : BaseApiController
    {
       public IActionResult Error(HttpStatusCode code)
       {
           return new ObjectResult(new ApiResponse(code));
       } 
    }
}