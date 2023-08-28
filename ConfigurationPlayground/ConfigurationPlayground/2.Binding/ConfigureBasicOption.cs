using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// TODO:`Is it good idea to use this namespace?
namespace ConfigurationPlayground._2.Binding;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddBasicOption(this IServiceCollection services, IConfiguration configuration)
    {
        // JustLocally(services, configuration);
        // LocalOverride(services, configuration);
        // WithConfigure(services, configuration);
        // WithBind(services, configuration);
        WithBindConfiguration(services);
        
        return services;
    }
    
    private static IServiceCollection GetSectionAndConfigure(IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<BasicOption>(options =>
            {
                // Get the section in IConfiguration
                var newOption = configuration.GetSection(BasicOption.SectionName).Get<BasicOption>();

                // Mutate the state
                options.Count = newOption.Count;
                
                // Don't update state if name is null
                if (!string.IsNullOrEmpty(newOption.Name))
                {
                    options.Name = newOption.Name;
                }
            });

        return services;
    }
    
    // Example on how we can chained configuration XYZ
    private static IServiceCollection ManualBinding(IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<BasicOption>(options =>
            {
                // Will mutate the state for you: will only update properties where the key is present in the section
                configuration.GetSection(BasicOption.SectionName).Bind(options);
            });

        return services;
    }
    
    private static IServiceCollection Exemple(IServiceCollection services, IConfiguration configuration)
    {
        /*
         * If in the IConfiguration we have: "Basic:Name"="B"
         *
         * The result will be:
         * basicOption.Name = "B"
         * basicOption.Count = 1
         */
        
        var basicOption = new BasicOption
        {
            Name = "A",
            Count = 1,
        };
        
        configuration.GetSection(BasicOption.SectionName).Bind(basicOption);
        
        return services;
    }
    
    private static IServiceCollection WithConfigure(IServiceCollection services, IConfiguration configuration)
    {
        // Same things
        services
            .Configure<BasicOption>(configuration.GetSection(BasicOption.SectionName));

        return services;
    }

    private static IServiceCollection WithBind(IServiceCollection services, IConfiguration configuration)
    {
        // Same things but different
        services.AddOptions<BasicOption>()
            .Bind(configuration.GetSection(BasicOption.SectionName));

        return services;
    }
    
    private static IServiceCollection WithBindConfiguration(IServiceCollection services)
    {
        // Same things but do not need IConfiguration anymore
        services.AddOptions<BasicOption>()
            .BindConfiguration(BasicOption.SectionName);

        return services;
    }
}