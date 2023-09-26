using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._7.OptionsFactory;

public class SpecificFactoryConfigurators : IConfigureNamedOptions<FactoryOptions>
{
    private readonly IConfiguration _configuration;

    // IConfiguration is always available in the CI
    public SpecificFactoryConfigurators(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void Configure(FactoryOptions options)
    {
        this.Configure(Options.DefaultName, options);
    }

    public void Configure(string? name, FactoryOptions options)
    {
        if (name == null)
        {
            return;
        }

        options.Name = name;
        
        // If specific option for the given name, it will update the options
        this._configuration.Bind($"{FactoryOptions.SpecificSectionName}:{name}", options);
    }
}