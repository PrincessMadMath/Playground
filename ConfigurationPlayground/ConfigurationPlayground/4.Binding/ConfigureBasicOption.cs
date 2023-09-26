using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationPlayground._3.Binding;

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
    
    // 1. We want to manually extract data from IConfiguration and configure an Options with it 
    private static IServiceCollection GetSectionAndConfigure(IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<BasicOption>(options =>
            {
                // Remember that configuration is a key/value pair of string
                var countStr = configuration[$"{BasicOption.SectionName}:Count"];
                if (Int32.TryParse(countStr, out var count))
                {
                    // Mutate the state
                    options.Count = count;
                }
                
                var nameStr = configuration[$"{BasicOption.SectionName}:Name"];
                
                // Don't update state if name is null
                if (nameStr != null)
                {
                    options.Name = nameStr;
                }
            });

        return services;
    }
    
    // 2. Let use the more friendly Get<>: is the behavior the same?
    private static IServiceCollection GetSectionAndConfigure2(IServiceCollection services, IConfiguration configuration)
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
    
    // 3. Sneak peak at Bind!
    private static IServiceCollection Exemple(IServiceCollection services, IConfiguration configuration)
    {
        var basicOption = new BasicOption
        {
            Name = "A",
            Count = 1,
        };
        
        // The section will contains: "Name"="B"
        configuration.GetSection(BasicOption.SectionName).Bind(basicOption);
        
        /*
         * The result will be:
         * basicOption.Name = "B"
         * basicOption.Count = 1
         */
        
        return services;
    }
    
    // 4. Let's put bind in a configure delegate!
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
    
    // 5. MORE SUGAR SYNTAX!
    private static IServiceCollection WithConfigure(IServiceCollection services, IConfiguration configuration)
    {
        // Same things
        services
            .Configure<BasicOption>(configuration.GetSection(BasicOption.SectionName));

        return services;
    }

    // 5. MOOOORE!!!! 
    private static IServiceCollection WithBind(IServiceCollection services, IConfiguration configuration)
    {
        // Same things but different
        services.AddOptions<BasicOption>()
            .Bind(configuration.GetSection(BasicOption.SectionName));

        return services;
    }
    
    // 6. Pls yeet GetSection + introduce a bug with named options pls!
    private static IServiceCollection WithBindConfiguration(IServiceCollection services)
    {
        // Same things but do not need IConfiguration anymore
        services.AddOptions<BasicOption>()
            .BindConfiguration(BasicOption.SectionName);

        return services;
    }
}