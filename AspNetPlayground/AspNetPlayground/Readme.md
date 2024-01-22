# What to remember

Playground to discover and play with some ASP.NET Core features.


## ASP Net Dev certificate

Dotnet offer a tools to create and trust a development certificate. (https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs)

Enough for simple use case, but for more complex setup you might want to load your own shared certificates.
(TODO: Sees how it's done in scaffoldit)



## Controller

Minimal API vs Controller-Base

https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0

### BaseController and [ApiController] for an API

BaseController:
* We use BaseController instead of Controller since we don't need the view part of the controller.
* BaseController offer many useful method (this.OK(), this.PhysicalFile(), this.TryValidateModel(), etc.)

ApiController:
* Automatic Model Validation: will returns 400 (ProblemDetails format)
* Binding Source Inference: making them implicit (see rules on the documentation)
* Make the [Route] attribute required
* Makes Model validation automatically: will returns 400 (ProblemDetails format)



### OpenAPI (Swagger)

See ``OpenAPI`` folder.

Good to know: For minimal API use this: [Microsoft.AspNetCore.OpenApi)](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-8.0)

### Response

- Caching
- Writing vs stream2

* Remember that we send Header and then Body: you can't update Header after you started to write the Body. You can hook on context.Response.OnStarting() if want to manipulate headers
* Some guide: https://code-maze.com/using-streams-with-httpclient-to-improve-performance-and-memory-usage/ + https://www.stevejgordon.co.uk/using-httpcompletionoption-responseheadersread-to-improve-httpclient-performance-dotnet + https://www.tugberkugurlu.com/archive/efficiently-streaming-large-http-responses-with-httpclient
* GetAsync/PostAsync wait to read the entire response body into (put into a MemoryBuffer) before returning control to the caller. At this point the TCP connection goess idle and become available for other request. This could be problematic for large body.
* When using the ResponseHeaderRead option the HttpClient will return control to the caller as soon as the response headers are read, the body might not have been receive completely. Also avoid thee intermediate MemoryStream buffer, will get content directly from the socket (?), can also leverage process that can read partial data (JSON). But since it maintain OS ressource, need handle disposable.

### Error Handling

#### ProblemDetails

Come from the need of standardize error response. (https://datatracker.ietf.org/doc/html/rfc7807)

- Will catch exception and return a ProblemDetails object
- Can also return a ProblemDetails object directly

More details: https://timdeschryver.dev/blog/translating-exceptions-into-problem-details-responses#default-problem-detail-extensions

#### 