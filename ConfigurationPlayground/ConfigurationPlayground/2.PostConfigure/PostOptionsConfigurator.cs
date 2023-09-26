using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._3.PostConfigure;

public class PostOptionsConfigurator: IPostConfigureOptions<PostOptions>
{
    public void PostConfigure(string? name, PostOptions options)
    {
        options.Value = "A postConfiguration will always be the last one, except if multiple PostConfiguration are registered, then far west!";
    }
}