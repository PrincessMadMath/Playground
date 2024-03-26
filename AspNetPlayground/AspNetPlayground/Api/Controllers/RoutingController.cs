using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RoutingController : ControllerBase
{
    private readonly EndpointDataSource _endpointDataSource;

    public RoutingController(EndpointDataSource endpointDataSource)
    {
        this._endpointDataSource = endpointDataSource;
    }

    [HttpGet]
    [Route("all-endpoints")]
    public IActionResult Get()
    {
        return this.Ok(this._endpointDataSource.Endpoints);
    }
}
