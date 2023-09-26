using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._5.NamedOptions;

public class OnlyDefaultNameConfigure: IConfigureOptions<NamedOptions>
{
    public void Configure(NamedOptions options)
    {
        options.Subject = "Default subject";
    }
}