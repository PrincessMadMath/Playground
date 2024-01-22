using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProblemDetailsController: ControllerBase
{
    [HttpGet]
    [Route("throw-exception")]
    public IActionResult ThrowException()
    {
        // Will get interceptedby the ProblemDetails middleware
        throw new InvalidOperationException("This is an example error.");
    }
    
    [HttpGet]
    [Route("bad-request")]
    public IActionResult ReturnBadRequest()
    {
        return this.BadRequest();
    }
    
    [HttpPost]
    [Route("post-with-validation")]
    public IActionResult ReturnBadRequest(ExampleModel model)
    {
        return this.BadRequest("This is an example error.");
    }
        
    public class ExampleModel
    {
        [Range(0, 10)]
        public int Count { get; set; }
    }
    
    [HttpGet]
    [Route("return-problem")]
    public IActionResult ReturnProblem()
    {
        // Alternatively, you can explicitly create a ProblemDetails response
        return Problem("This is an example error.", statusCode: 500);
    }
}