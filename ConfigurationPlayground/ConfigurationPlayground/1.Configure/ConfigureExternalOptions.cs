using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._1.Configure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalOptions(this IServiceCollection services)
    {
        // No need to bind to IConfiguration to add an Options
        services
            .AddOptions<ExternalOptions>()
            .Configure(options =>
            {
                // Important: I mutate the option here!
                options.ConfiguredOptions =
                    "I'm going to get overwritten by ConfigureConfigurableOptions since order matters";
            });

        // I can add a configure later (maybe conditional if in dev environment)
        services.Configure<ExternalOptions>(options =>
        {
            // Still mutating here!
            options.ConfiguredOptions = $"I can sneak a surprise configuration much later!, previous value: {options.ConfiguredOptions}";
        });
        
        // Using the IConfigureOptions injection which can resolve DI dependencies
        services.AddSingleton<ConfigureServices>();
        services.AddSingleton<IConfigureOptions<ExternalOptions>, ExternalOptionsConfigurator>();

        return services;
    }
}