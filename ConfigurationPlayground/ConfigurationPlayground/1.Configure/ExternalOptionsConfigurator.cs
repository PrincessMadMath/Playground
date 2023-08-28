using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._1.Configure;

public class ExternalOptionsConfigurator: IConfigureOptions<ExternalOptions>
{
    private readonly ConfigureServices _configureServices;

    public ExternalOptionsConfigurator(ConfigureServices configureServices)
    {
        _configureServices = configureServices;
    }
    
    public void Configure(ExternalOptions options)
    {
        // Again mutating!
        options.ConfiguredOptions = _configureServices.GetValue();
    }
}