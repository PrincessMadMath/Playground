using Microsoft.Extensions.Options;

namespace ConfigurationPlayground._5.NamedOptions;

public class AllNamedConfigure: IConfigureNamedOptions<NamedOptions>
{
    public void Configure(string? name, NamedOptions options)
    {
        // Only configure the options if this is the correct instance
        if (name == "Admin2")
        {
            options.Subject = "Admin subject 2";
        }
     
        if (name == Options.DefaultName)
        {
            Console.WriteLine("They are all named!!!");
        }
    }

    // Won't be called since factory cast in IConfigureNameOptions and use other methods
    public void Configure(NamedOptions options)
    {
        Configure(Options.DefaultName, options);
    }
}