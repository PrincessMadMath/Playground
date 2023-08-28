using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._9.DeadlockOfConfiguration;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddDeadlockConfiguration(this IServiceCollection services)
    {
        services.AddOptions<DeadLockOptionsA>()
            .BindConfiguration(DeadLockOptionsA.SectionName);
        
        services.AddOptions<DeadLockOptionsB>()
            .BindConfiguration(DeadLockOptionsB.SectionName);

        // Create a circular dependency and crash at startup :(        
        // services.AddSingleton<IConfigureOptions<DeadLockOptionsA>, DeadlockAConfigurator>();
        // services.AddSingleton<IConfigureOptions<DeadLockOptionsB>, DeadlockBConfigurator>();

        return services;
    }
}