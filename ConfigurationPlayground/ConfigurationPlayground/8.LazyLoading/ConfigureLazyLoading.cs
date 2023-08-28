using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._8.LazyLoading;

public static partial class ServiceCollectionExtensions
{
    
    public static IServiceCollection AddLazyOptions(this IServiceCollection services, IConfiguration configuration, Action<string> addJsonFile)
    {
        services.AddOptions<LazyOptions>()
            .BindConfiguration(LazyOptions.SectionName)
            .Configure(options =>
            {
                options.GuessMyValue = "From configure";
            });

        return services;
    }
    
    /// <summary>
    /// You should avoid using the IConfiguration or IOption before they are completely configured.
    /// The way to do it is:
    ///     - Receive an IOption in the ctor of your services
    ///     - Use the factory pattern when adding services to the DI 
    /// </summary>
    // public static IServiceCollection AddLazyOptions(this IServiceCollection services, IConfiguration configuration, Action<string> addJsonFile)
    // {
    //     services.AddOptions<LazyOptions>()
    //         .BindConfiguration(LazyOptions.SectionName)
    //         .Configure(options =>
    //         {
    //             options.GuessMyValue = $"Now: From configure #1, Previously: {options.GuessMyValue}";
    //         });
    //     
    //     var fromCapturedConfiguration = configuration.GetSection(LazyOptions.SectionName).Get<LazyOptions>().GuessMyValue;
    //     
    //     // Nono! Don't do this!
    //     using var sp = services.BuildServiceProvider();
    //
    //     addJsonFile("other.json");
    //     
    //     services.Configure<LazyOptions>(options =>
    //     {
    //         options.GuessMyValue = "From configure #2";
    //     });
    //
    //     // Why take into account other.json?
    //     var fromPrematureOption = sp.GetRequiredService<IOptionsMonitor<LazyOptions>>().CurrentValue.GuessMyValue;
    //     
    //     services.AddScoped<EmptyClass>(_ =>
    //     {
    //         return new EmptyClass("From captured configuration",fromCapturedConfiguration);
    //     });
    //     
    //     services.AddScoped<EmptyClass>(_ =>
    //     {
    //         return new EmptyClass("From premature options",fromPrematureOption);
    //     });
    //     
    //     services.AddScoped<EmptyClass>((services) =>
    //     {
    //         var lazyConfiguration2 = services.GetRequiredService<IConfiguration>().GetSection(LazyOptions.SectionName).Get<LazyOptions>().GuessMyValue;
    //         return new EmptyClass("From options in factory", lazyConfiguration2);
    //     });
    //     
    //     services.AddScoped<EmptyClass>(services =>
    //     {
    //         var lazyOptions2 = services.GetRequiredService<IOptions<LazyOptions>>().Value.GuessMyValue;
    //         return new EmptyClass("From options in factory", lazyOptions2);
    //     });
    //     
    //     services.Configure<LazyOptions>(options =>
    //     {
    //         options.GuessMyValue = "From configure #3";
    //     });
    //
    //
    //     return services;
    // }
    
    //     public static IServiceCollection AddLazyOptions(this IServiceCollection services, IConfiguration configuration, Action<string> addJsonFile)
    // {
    //     services.AddOptions<LazyOptions>()
    //         .BindConfiguration(LazyOptions.SectionName);
    //     
    //     var lazyConfiguration = configuration.GetSection(LazyOptions.SectionName).Get<LazyOptions>().GuessMyValue;
    //     
    //     // Nono! Don't do this!
    //     using var sp = services.BuildServiceProvider();
    //     var prematureLazyConfiguration1 = sp.GetRequiredService<IOptions<LazyOptions>>().Value.GuessMyValue;
    //     
    //     services.Configure<LazyOptions>(options =>
    //     {
    //         options.GuessMyValue = "From configure #1";
    //     });
    //     
    //     using var sp2 = services.BuildServiceProvider();
    //     var prematureLazyConfiguration2 = sp2.GetRequiredService<IOptions<LazyOptions>>().Value.GuessMyValue;
    //
    //     addJsonFile("other.json");
    //     var lazyConfigurationPostJson = configuration.GetSection(LazyOptions.SectionName).Get<LazyOptions>().GuessMyValue;
    //     
    //     services.AddScoped<EmptyClass>(services =>
    //     {
    //         return new EmptyClass("From captured configuration before json",lazyConfiguration);
    //     });
    //     
    //     services.AddScoped<EmptyClass>(services =>
    //     {
    //         return new EmptyClass("From premature options before configure",prematureLazyConfiguration1);
    //     });
    //     
    //     services.AddScoped<EmptyClass>(services =>
    //     {
    //         return new EmptyClass("From premature options after configure",prematureLazyConfiguration2);
    //     });
    //
    //     services.AddScoped<EmptyClass>(services =>
    //     {
    //         return new EmptyClass("From captured configuration after json",lazyConfigurationPostJson);
    //     });
    //     
    //     services.AddScoped<EmptyClass>((services) =>
    //     {
    //         var lazyConfiguration2 = services.GetRequiredService<IConfiguration>().GetSection(LazyOptions.SectionName).Get<LazyOptions>().GuessMyValue;
    //         return new EmptyClass("From options in factory", lazyConfiguration2);
    //     });
    //     
    //     services.AddScoped<EmptyClass>(services =>
    //     {
    //         var lazyOptions2 = services.GetRequiredService<IOptions<LazyOptions>>().Value.GuessMyValue;
    //         return new EmptyClass("From options in factory", lazyOptions2);
    //     });
    //
    //     services.Configure<LazyOptions>(options =>
    //     {
    //         options.GuessMyValue = "From configure #2";
    //     });
    //
    //
    //     return services;
    // }
}