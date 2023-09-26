using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._5.NamedOptions;

// https://andrewlock.net/configuring-named-options-using-iconfigurenamedoptions-and-configureall
public static partial class ServiceCollectionExtensions
{
    // 1. Register a couple of named options
    public static IServiceCollection AddNamedOption(this IServiceCollection services, IConfiguration configuration)
    {
        // They are all named!!!! Even the default one!
        services
            .AddOptions<NamedOptions>()
            .Bind(configuration.GetSection(NamedOptions.EmailSectionName))
            .ValidateOnStart();
        
        // Later configure of default
        services.Configure<NamedOptions>(options =>
        {
            options.Subject = "Later configured default options";
        });


        // If I do not named them this AddOption will override the previous one
        services
            .AddOptions<NamedOptions>("Admin")
            .Bind(configuration.GetSection(NamedOptions.AdminEmailSectionName))
            .ValidateOnStart();

        // Later configure of named
        services.Configure<NamedOptions>("Admin", options =>
        {
            options.Subject = "Later configured Admin-named options";
        });
        
        // I can bind to the same section if I want!
        services
            .AddOptions<NamedOptions>("Admin2")
            .Bind(configuration.GetSection(NamedOptions.AdminEmailSectionName))
            .ValidateOnStart();
        
        // Of a different one
        services
            .AddOptions<NamedOptions>("HR")
            .Bind(configuration.GetSection(NamedOptions.HrEmailSectionName))
            .ValidateOnStart();
        
        // Can add IConfigureOptions for only default one
        services.AddSingleton<IConfigureOptions<NamedOptions>, OnlyDefaultNameConfigure>();
        
        // Can add a IConfigureOptions for all named options (including the default name)
        // Important: note that you must register as an IConfigureOptions<T> instance, not an IConfigureNamedOptions<T> instance
        // Remember the OptionsFactory we saw earlier
        services.AddSingleton<IConfigureOptions<NamedOptions>, AllNamedConfigure>();

        // Only register a IConfigureNamedOptions without filter on naming
        services.ConfigureAll<NamedOptions>(options =>
        {
            Console.WriteLine("Running for all from a delegate!");
        });

        return services;
    }
}