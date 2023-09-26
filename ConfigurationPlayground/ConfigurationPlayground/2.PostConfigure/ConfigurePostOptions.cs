using ConfigurationPlayground._1.Configure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._3.PostConfigure;

/// <summary>
/// PostConfigure is a useful technique to have available when you want to make sure configuration provided by users
/// is valid for your application, allowing you to hook into the process, once the configuration has been established
/// but before it is used, to make any necessary updates.
///
/// Seems to mostly be useful for library authors who don't have control on when a client will configure relevant config
///
/// Source:
/// - https://www.andybutland.dev/2023/02/use-postconfigure-for-default.html
/// - https://andrewlock.net/delaying-strongly-typed-options-configuration-using-postconfigure-in-asp-net-core/
/// </summary>
public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostOptions(this IServiceCollection services)
    {
        services.AddOptions<PostOptions>()
            .PostConfigure(options =>
            {
                options.Value = "I am a post configure so I will stay until the end no?";
            });
        
        services.PostConfigure<PostOptions>(options => 
            options.Value = "Order is still important here, the next PostConfigure will win :("
        );
        
        services.AddSingleton<IPostConfigureOptions<PostOptions>, PostOptionsConfigurator>();

        services.Configure<PostOptions>(options => 
            options.Value = "Even if after PostOption I will get overwritten"
        );

        return services;
    }
}