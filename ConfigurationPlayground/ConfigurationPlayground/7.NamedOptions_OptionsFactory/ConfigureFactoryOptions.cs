using System.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._7.OptionsFactory;

public static partial class ServiceCollectionExtensions
{
    // When is the Options is "compute"/configure
    //      - At startup
    //      - When controller that require this Options get instantiated?
    //      - When accessing the options?
    // What if I tell you can request a named options instance that has not been explicitly registered. :O
    public static IServiceCollection AddFactoryOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureAll<FactoryOptions>(options =>
        {
            configuration.GetSection(FactoryOptions.DefaultSectionName).Bind(options);
        });

        services.AddSingleton<IConfigureOptions<FactoryOptions>, SpecificFactoryConfigurators>();

        return services;
    }
}