using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._1.Configure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalOptions(this IServiceCollection services)
    {
        // 1. No need to bind to IConfiguration to add an Options
        services
            // 2. Let's check what AddOptions is doing! + implementation details!
            //      - Notice Default Name
            //      - Guess how will be registered IOptions, IOptionsSnapshot, IOptionsMonitor (spot IOptionFactory)? => look implementation
            //      - Notice the type only serve to create a named builder here
            .AddOptions<ExternalOptions>()
            .Configure(options =>
            {
                // Important: I mutate the option here!
                options.ConfiguredOptions =
                    "I'm going to get overwritten by ConfigureConfigurableOptions since order matters";
            });

        // 3. I can add a configure later (maybe conditional if in dev environment)
        //          - Here Configure is a method on IServiceCollection and not OptionsBuilder (let's look into the difference)
        services.Configure<ExternalOptions>(options =>
        {
            // Still mutating here!
            options.ConfiguredOptions = $"I can sneak a surprise configuration much later!, previous value: {options.ConfiguredOptions}";
        });
        
        // 4. Using the IConfigureOptions injection which can resolve DI dependencies
        services.AddSingleton<ConfigureServices>();
        services.AddSingleton<IConfigureOptions<ExternalOptions>, ExternalOptionsConfigurator>();

        return services;
    }
}