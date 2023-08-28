using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._4.ValidatedOptions;

// https://code-maze.com/aspnet-configuration-options-validation/
public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidatedOption(this IServiceCollection services)
    {
        // Using IValidateOptions
        // For complex scenario
        // TODO can it get injected things?
        services
            .AddOptions<ValidatedOptions>()
            .BindConfiguration(ValidatedOptions.SectionName)
            .ValidateDataAnnotations() // Using Data Annotation duh
            .Validate(options => // Using delegate = allow more flexibility
            {
                if (options.NumberA + options.NumberB != options.Sum)
                {
                    return false;
                }
        
                return true;
        
            }, "Sum is not correct") // Can return error message
            .ValidateOnStart(); // Validate when application start not now
        
        // The IValidateOptions can be injected services from the DI
        services.AddSingleton<InjectedValidator>();
        services.AddSingleton<IValidateOptions<ValidatedOptions>, OptionValidator>();
        
        return services;
    }
}

// TODO: Show example with validation and named options