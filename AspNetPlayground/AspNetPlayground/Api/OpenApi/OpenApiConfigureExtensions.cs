﻿using Microsoft.OpenApi.Models;

namespace Api.OpenApi;

public static class OpenApiConfigureExtensions
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        // Enable collecting endpoints metadata, it will be use by Swagger
        services.AddEndpointsApiExplorer();
        
        // Will generate a OpenAPI specification file
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations(); // Alternative to xml documentation
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"});
        });
        return services;
    }
    
    public static IApplicationBuilder UseOpenApi(this WebApplication app)
    {

        if (!app.Environment.IsProduction())
        {
            // Add Swagger middleware that can generate the OpenApi Specification file
            app.UseSwagger();
            
            // Add Swagger UI middleware that provide a UI to display interactive documentation
            app.UseSwaggerUI();
        }

        return app;
    }
}