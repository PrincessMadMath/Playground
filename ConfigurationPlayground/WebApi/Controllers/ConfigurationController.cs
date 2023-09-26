using ConfigurationPlayground._0.ConfigurationSource;
using ConfigurationPlayground._1.Configure;
using ConfigurationPlayground._3.Binding;
using ConfigurationPlayground._3.PostConfigure;
using ConfigurationPlayground._4.ValidatedOptions;
using ConfigurationPlayground._5.NamedOptions;
using ConfigurationPlayground._6.NamedOptionsRefreshBug;
using ConfigurationPlayground._7.OptionsFactory;
using ConfigurationPlayground._8.LazyLoading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers;

[ApiController]
[Route("configuration")]
public class ConfigurationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IOptionsMonitor<SourceOptions> _sourceOptions;
    private readonly IOptionsSnapshot<BasicOption> _basicOptions;
    private readonly IOptionsSnapshot<ValidatedOptions> _validatedOptions;
    private readonly IOptionsSnapshot<NamedOptions> _namedOptions;
    private readonly IOptionsMonitor<ProblemNamedOptions> _problemNamedOptionsMonitor;
    private readonly IOptionsSnapshot<ProblemNamedOptions> _problemNamedOptions;
    private readonly IOptionsSnapshot<ExternalOptions> _configurableOptions;
    private readonly IOptionsSnapshot<PostOptions> _postOptions;
    private readonly IOptionsSnapshot<FactoryOptions> _factoryOptions;
    private readonly IOptionsFactory<FactoryOptions> _factoryFactory;
    private readonly IOptionsMonitor<LazyOptions> _lazyOptions;
    private readonly IEnumerable<EmptyClass> _emptyClasses;

    public ConfigurationController(
        IConfiguration configuration,
        IOptionsMonitor<SourceOptions> sourceOptions,
        IOptionsSnapshot<BasicOption> basicOptions, 
        IOptionsSnapshot<ValidatedOptions> validatedOptions, 
        IOptionsSnapshot<NamedOptions> namedOptions,
        IOptionsMonitor<ProblemNamedOptions> problemNamedOptionsMonitor,
        IOptionsSnapshot<ProblemNamedOptions> problemNamedOptions,
        IOptionsSnapshot<ExternalOptions> configurableOptions,
        IOptionsSnapshot<PostOptions> postOptions,
        IOptionsSnapshot<FactoryOptions> factoryOptions,
        IOptionsFactory<FactoryOptions> factoryFactory,
        IOptionsMonitor<LazyOptions> lazyOptions,
        IEnumerable<EmptyClass> emptyClasses)
    {
        _configuration = configuration;
        _sourceOptions = sourceOptions;
        _basicOptions = basicOptions;
        _validatedOptions = validatedOptions;
        _namedOptions = namedOptions;
        this._problemNamedOptionsMonitor = problemNamedOptionsMonitor;
        _problemNamedOptions = problemNamedOptions;
        _configurableOptions = configurableOptions;
        _postOptions = postOptions;
        _factoryOptions = factoryOptions;
        _factoryFactory = factoryFactory;
        _lazyOptions = lazyOptions;
        _emptyClasses = emptyClasses;
    }
    
    [HttpGet("source")]
    public ActionResult<SourceOptions> GetSourceOptions()
    {
        return this.Ok(this._sourceOptions.CurrentValue);
    }

    [HttpGet("basic")]
    public ActionResult<BasicOption> GetBasicOptions()
    {
        return this.Ok(this._basicOptions.Value);
    }
    
    [HttpGet("validatedOptions")]
    public ActionResult<ValidatedOptions> GetValidatedOptions()
    {
        return this.Ok(this._validatedOptions.Value);
    }
    
    [HttpGet("namedOptions")]
    public ActionResult<NamedOptions> GetNamedOptions()
    {
        // Wait it's all named options? 🔫 Always has been. 
        var emailOption = this._namedOptions.Value;
        var emptyNamed = this._namedOptions.Get(string.Empty);
        var adminNamed = this._namedOptions.Get("Admin");
        var hrNamed = this._namedOptions.Get("HR");
        
        return this.Ok(new 
        {
            emailOption,
            emptyNamed,
            adminNamed,
            hrNamed,
        });
    }
    
    [HttpGet("problemNamedOptions")]
    public ActionResult<ProblemNamedOptions> GetProblemNamedOptions()
    {
        var namedOptionFromSnapshot = this._problemNamedOptions.Get("OHOH");
        var namedOptionFromMonitor = this._problemNamedOptionsMonitor.Get("OHOH");
        
        var namedOptionFromSnapshot2 = this._problemNamedOptions.Get("OHOH2");
        var namedOptionFromMonitor2 = this._problemNamedOptionsMonitor.Get("OHOH2");
        
        return this.Ok(new 
        {
            namedOptionFromSnapshot,
            namedOptionFromMonitor,
            namedOptionFromSnapshot2,
            namedOptionFromMonitor2
        });
    }
    
    [HttpGet("configurableOptions")]
    public ActionResult<ExternalOptions> GetConfigurableOptions()
    {
        var configurableOptions = this._configurableOptions.Value;
        
        return this.Ok(new 
        {
            configurableOptions
        });
    }
    
    [HttpGet("postOptions")]
    public ActionResult<PostOptions> GetPostOptions()
    {
        var postOptions = this._postOptions.Value;
        
        return this.Ok(new 
        {
            configurableOptions = postOptions
        });
    }
    
    [HttpGet("factoryOptions")]
    public ActionResult<FactoryOptions> GetFactoryOptions()
    {
        var defaultOptions = this._factoryOptions.Value;
        
        // OptionsManager will check is cache of options and if it does not exist will use the factory to create it
        var namedWorkleapOptions = this._factoryOptions.Get("Workleap");
        // Will create the same result but the result won't be re-used
        var builtdWorkleapOptions = this._factoryFactory.Create("Workleap");
        var builtdOtherOptions = this._factoryFactory.Create("Nintendo");
        
        return this.Ok(new 
        {
            defaultOptions,
            namedWorkleapOptions,
            builtdWorkleapOptions,
            builtdOtherOptions,
        });
    }
    
    [HttpGet("lazyLoading")]
    public ActionResult<FactoryOptions> TriggerScopedEmptyClasses()
    {
        var rawConfiguration = this._configuration[$"{LazyOptions.SectionName}:GuessMyValue"];
        
        var fromConfiguration = this._configuration
            .GetSection(LazyOptions.SectionName).Get<LazyOptions>()?.GuessMyValue;
        
        var fromOptions = this._lazyOptions.CurrentValue.GuessMyValue;
        
        Console.WriteLine($"From configuration: {fromConfiguration}");
        Console.WriteLine($"From options: {fromOptions}");
        
        
        
        
        
        Console.WriteLine("---------------------------------");
        return this.Ok();
    }
}