using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._4.ValidatedOptions;

// https://code-maze.com/aspnet-configuration-options-validation/
public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidatedOption(this IServiceCollection services)
    {
        services
            .AddOptions<ValidatedOptions>()
            .BindConfiguration(ValidatedOptions.SectionName) // Ignore Bind for now
            .ValidateDataAnnotations() // 1. Let's peek inside! (only register IValidateOptions)
            .Validate(options => // 2. Can still use delegate for a bit more allow more flexibility, peek!
            {
                if (options.NumberA + options.NumberB != options.Sum)
                {
                    return false;
                }
        
                return true;
        
            }, "Sum is not correct") // Can return error message
            .ValidateOnStart(); // 4. Register a hosted service + configure an other option with type + name meta!
        
        // 3. Can also manually register a IValidateOptions which will give access to the DI
        services.AddSingleton<InjectedValidator>();
        services.AddSingleton<IValidateOptions<ValidatedOptions>, OptionValidator>();
        
        return services;
    }
}

// TODO: Show example with validation and named options