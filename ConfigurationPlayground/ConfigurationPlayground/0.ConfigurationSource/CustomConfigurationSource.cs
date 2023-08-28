using Microsoft.Extensions.Configuration;

namespace ConfigurationPlayground._0.ConfigurationSource;

public class CustomConfigurationSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new CustomConfigurationProvider();
    }
}