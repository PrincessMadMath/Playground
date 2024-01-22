using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloWorldController: ControllerBase
{
    [HttpGet]
    [Route("hello")]
    [SwaggerOperation(Summary = "Hello world from a controller!")]
    [ProducesResponseType<string>(200)]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }
}