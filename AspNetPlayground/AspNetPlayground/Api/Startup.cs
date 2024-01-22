using Api.HttpClients;
using Api.OpenApi;

namespace Api;

public static class Startup
{
    public static IHostApplicationBuilder RegisterDependencies(this IHostApplicationBuilder builder)
    {
        // Enable to discovers controllers (vs minimal API)
        builder.Services.AddControllers();

        // ** Error Handling **
        // Can be extended to include other parameters by default
        builder.Services.AddProblemDetails();
        
        builder.Services.AddOpenApi();

        builder.Services.AddHttpClients();
        
        return builder;
    }


    // TODO: Is it only middleware or something pipelines?
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        // TODO: Why is it not required?
        // app.UseRouting();

        // ** Error Handling **
        
        // Translate exception to Problem Details (exclude some sensitive information)
        // TODO: More to it!
        app.UseExceptionHandler();

        // Convert any response with a status code between 400 and 599 without a body to a ProblemDetails response
        app.UseStatusCodePages();
        
        // TODO
        if (app.Environment.IsDevelopment())
        {
            // Will add the exception details to the response (callstack, headers, etc)
            app.UseDeveloperExceptionPage();
        }
        
        // TODO: Health check
        
        app.UseOpenApi();
        
        // Minimal API Example
        app
            .MapGet("/", () => "Hello World!")
            .WithName("GetHelloWorld")
            .WithSummary("Hello world from a minimal API!")
            .WithOpenApi();
        
        // Controllers Example
        app.MapControllers();
        
        
        return app;
    }

}