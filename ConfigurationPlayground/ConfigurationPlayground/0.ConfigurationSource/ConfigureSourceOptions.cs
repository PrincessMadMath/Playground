using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationPlayground._0.ConfigurationSource;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSourceOption(this IServiceCollection services)
    {
        services.AddOptions<SourceOptions>()
            .BindConfiguration(SourceOptions.SectionName);

        return services;
    }
}