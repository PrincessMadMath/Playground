using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._3.PostConfigure;

public class PostOptionsConfigurator: IPostConfigureOptions<PostOptions>
{
    public void PostConfigure(string? name, PostOptions options)
    {
        options.Value = "PostConfiguration will always be the last one, except if multiple PostConfiguration are registered";
    }
}