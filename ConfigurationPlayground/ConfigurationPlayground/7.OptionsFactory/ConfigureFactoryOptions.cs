using System.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._7.OptionsFactory;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddFactoryOptions(this IServiceCollection services, IConfiguration configuration)
    {
        // Can't use a Bind or BindConfigured since it associated to only 1 named options
        services.ConfigureAll<FactoryOptions>(options =>
        {
            configuration.GetSection(FactoryOptions.DefaultSectionName).Bind(options);
        });

        services.AddSingleton<IConfigureOptions<FactoryOptions>, SpecificFactoryConfigurators>();

        return services;
    }
}