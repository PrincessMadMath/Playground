using ConfigurationPlayground._0.ConfigurationSource;
using ConfigurationPlayground._1.Configure;
using ConfigurationPlayground._3.Binding;
using ConfigurationPlayground._3.PostConfigure;
using ConfigurationPlayground._4.ValidatedOptions;
using ConfigurationPlayground._5.NamedOptions;
using ConfigurationPlayground._6.NamedOptionsRefreshBug;
using ConfigurationPlayground._7.OptionsFactory;
using ConfigurationPlayground._8.LazyLoading;
using ConfigurationPlayground._9.DeadlockOfConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/* Can add Configuration Sources (IConfigurationSource)
 - Remember that by default, there is already source registered and the order is important! 
configuration.AddJsonFile("other.json", optional: true, reloadOnChange: true);
 */

// 1. Check implementation of custom Configuration Provider
// When will the data get loaded?
builder.Configuration.Add<CustomConfigurationSource>(null);

// Let's see how recover the value of a key? 1 dictionary or check all providers?
var customKey = builder.Configuration["Source:CustomKey"];


// Support dynamic configuration from Azure App Config
// Could add support to reference from Key Vault
// 
// var connectionString = builder.Configuration["Azure:ConnectionString"];
// builder.Configuration.AddAzureKeyVault("vault");
// builder.Configuration.AddAzureAppConfiguration(options =>
// {
//     options.Connect(new Uri("connection string"), new DefaultAzureCredential())
//         .Select(KeyFilter.Any)
//         .ConfigureRefresh(refreshOptios => refreshOptios.Register("SentinelKey", true));
// });


// 2. Configure Options
builder.Services.AddSourceOption();
builder.Services.AddBasicOption(builder.Configuration);
builder.Services.AddValidatedOption();
builder.Services.AddNamedOption(builder.Configuration);
builder.Services.AddProblemNamedOption(builder.Configuration);
builder.Services.AddExternalOptions();
builder.Services.AddPostOptions();
builder.Services.AddFactoryOptions(builder.Configuration);
builder.Services.AddLazyOptions(builder.Configuration, (file) =>
{
    builder.Configuration.AddJsonFile(file, optional: true, reloadOnChange: true);
});
builder.Services.AddDeadlockConfiguration();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
 * After that point you can't
 * - Add anything in the DI
 * - Change option configuration
 */
var app = builder.Build();


// TODO: What to do before or after the build?

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();