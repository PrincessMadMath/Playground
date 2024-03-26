using System.ComponentModel.DataAnnotations;
using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// [ApiController] is doing a lots of nice things for us:
/// - Automatically validate the request
/// - Automatically return a 400 Bad Request if the request is invalid
/// - Automatically return a 500 Internal Server Error if an exception is thrown (ProblemDetails in Development, Empty body in Production)
/// </summary>
[ApiController]
[Route("[controller]")]
public class OpenApiController : ControllerBase
{

    [HttpGet]
    [Route("get-example")]
    [ProducesResponseType<ExampleResponse>((int)HttpStatusCode.OK)]
    [ProducesResponseType<ProblemDetails>((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.TooManyRequests)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public IActionResult Get([FromQuery] ExampleRequest request)
    {
        switch (request.Type)
        {
            case ResponseType.Ok:
                return this.Ok(new ExampleResponse { Message = request.Count.ToString() });
            case ResponseType.BadRequest:
                return this.Problem("This is a bad request.", statusCode: (int)HttpStatusCode.BadRequest);
            case ResponseType.TooManyRequests:
                // Missing how to add Retry-After header description in OpenAPI
                this.Response.Headers.Append("Retry-After", "120");
                return this.StatusCode((int)HttpStatusCode.TooManyRequests);
            case ResponseType.Exception:
                throw new Exception("This is an example error.");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [HttpPost]
    [Route("post-example")]
    public IActionResult Post(ExampleRequest request)
    {
        switch (request.Type)
        {
            case ResponseType.Ok:
                return this.Ok(new ExampleResponse { Message = request.Count.ToString() });
            case ResponseType.BadRequest:
                return this.Problem("This is a bad request.", statusCode: (int)HttpStatusCode.BadRequest);
            case ResponseType.TooManyRequests:
                this.Response.Headers.Append("Retry-After", "120");
                return this.StatusCode((int)HttpStatusCode.TooManyRequests);
            case ResponseType.Exception:
                throw new Exception("This is an example error.");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public class ExampleRequest
    {
        [Required]
        public ResponseType Type { get; set; }

        [Required]
        [Range(0, 10)]
        public int Count { get; set; }
    }

    public enum ResponseType
    {
        Ok,
        BadRequest,
        TooManyRequests,
        Exception,
    }

    public class ExampleResponse
    {
        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
