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
        // Route Matching Middleware: find best endpoint based on the request
        // When using WebApplicationBuilder it is automatically added
        // app.UseRouting();
        
        // Add endpoints execution to the middleware pipeline
        // When using WebApplicationBuilder it is automatically added
        // app.UseEndpoints();

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
        
        if (!app.Environment.IsDevelopment())
        {
            // Enforce HTTPS
            app.UseHsts();
        }
        
        // TODO: Health check
        
        app.UseOpenApi();
        
        // Minimal API Example
        // Define an endpoint: something that can be selected by matching URL and executed
        app
            .MapGet("/", () => "Hello World!")
            .WithName("GetHelloWorld")
            .WithSummary("Hello world from a minimal API!")
            //.RequireAuthorization() // You can add metadata to endpoint which can be used by middleware
            .WithOpenApi();
        
        // Define endpoints for convention based controllers
        app.MapControllers();
        
        
        return app;
    }

}