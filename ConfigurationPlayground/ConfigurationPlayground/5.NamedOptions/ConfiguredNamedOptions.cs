using Microsoft.Extensions.DependencyInjection;

// TODO:`Is it good idea to use this namespace?
namespace ConfigurationPlayground._5.NamedOptions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddNamedOption(this IServiceCollection services)
    {
        services
            .AddOptions<NamedOptions>()
            .BindConfiguration(NamedOptions.EmailSectionName)
            .ValidateOnStart();

        // If I do not named them this AddOption will override the previous one
        services
            .AddOptions<NamedOptions>("Admin")
            .BindConfiguration(NamedOptions.AdminEmailSectionName)
            .ValidateOnStart();
        
        services
            .AddOptions<NamedOptions>("HR")
            .BindConfiguration(NamedOptions.HrEmailSectionName)
            .ValidateOnStart();

        return services;
    }
}