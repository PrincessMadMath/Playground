using System.Text.Json;
using System.Text.Json.Serialization;
using Api.HttpClients;
using Api.OpenApi;
using Microsoft.AspNetCore.Mvc;

namespace Api;

public static class Startup
{
    public static IHostApplicationBuilder RegisterDependencies(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.Filters.Add(new ConsumesAttribute("application/json"));

        });
        
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            
        });

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
        if (!app.Environment.IsDevelopment())
        {
            // Enforce HTTPS
            app.UseHsts();
        }
        
        // TODO: Health check
        
        app.UseOpenApi();

        // Define endpoints for convention based controllers
        app.MapControllers();
        
        
        return app;
    }
    
    public static WebApplication ConfigureExperimentalMiddleware(this WebApplication app)
    {
        // Route Matching Middleware: find best endpoint based on the request
        // When using WebApplicationBuilder it is automatically added
        app.UseRouting();
        
        // Add endpoints execution to the middleware pipeline
        // When using WebApplicationBuilder it is automatically added
        // Old school way of doing this, should now do app.MapControllers()
#pragma warning disable ASP0014
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
#pragma warning restore ASP0014

        // ** Error Handling **
        
        // Would redirect use to /error, not wanted for API
        app.UseExceptionHandler("/error");

        // Add a simple body to the response when there is an error and no body
        app.UseStatusCodePages();
        
        // Return a HTML page with exception details: callstack, headers, etc,...
        // Weird for an API
        //app.UseDeveloperExceptionPage();

        // Minimal API Example
        // Define an endpoint: something that can be selected by matching URL and executed
        app
            .MapGet("/", () => "Hello World!")
            .WithName("GetHelloWorld")
            .WithSummary("Hello world from a minimal API!")
            //.RequireAuthorization() // You can add metadata to endpoint which can be used by middleware
            .WithOpenApi();
        
        return app;
    }

}