using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// TODO:`Is it good idea to use this namespace?
namespace ConfigurationPlayground._6.NamedOptionsRefreshBug;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddProblemNamedOption(this IServiceCollection services, IConfiguration configuration)
    {
        // If I do not named them this AddOption will override the previous one
        // services
        //     .AddOptions<ProblemNamedOptions>("OHOH")
        //     .BindConfiguration(ProblemNamedOptions.SectionName)
        //     .Validate(options =>
        //     {
        //         // Problem #1: Does not get call on change
        //         if (string.IsNullOrEmpty(options.Value))
        //         {
        //             return false;
        //         }
        //
        //         return true;
        //     })
        //     .ValidateOnStart();
        
        // When using Bind it register correctly the named options in the onChange callback
        services
            .AddOptions<ProblemNamedOptions>("OHOH")
            .Bind(configuration.GetSection(ProblemNamedOptions.SectionName))
            .Validate(options =>
            {
                if (string.IsNullOrEmpty(options.Value))
                {
                    return false;
                }

                return true;
            })
            .ValidateOnStart();
        
        services.AddSingleton<IValidateOptions<ProblemNamedOptions>, ProblemNamedOptionValidator>();

        return services;
    }
}