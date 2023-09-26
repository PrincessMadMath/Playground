using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// TODO:`Is it good idea to use this namespace?
namespace ConfigurationPlayground._6.NamedOptionsRefreshBug;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddProblemNamedOption(this IServiceCollection services, IConfiguration configuration)
    {
        // Using BindConfiguration won't configure properly the OptionsChangeTokenSource
        // Won't validate with the right configuration since will build the options with the default value
        // IOptionsMonitor won't get updated correctly 
        services
            .AddOptions<ProblemNamedOptions>("OHOH")
            .BindConfiguration(ProblemNamedOptions.SectionName)
            .Validate(options =>
            {
                // Problem #1: Does not get call on change
                if (string.IsNullOrEmpty(options.Value))
                {
                    return false;
                }
        
                return true;
            })
            .ValidateOnStart();
        
        // When using Bind it register correctly the named options in the onChange callback
        services
            .AddOptions<ProblemNamedOptions>("OHOH2")
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