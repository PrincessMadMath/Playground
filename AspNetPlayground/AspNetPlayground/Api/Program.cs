using Api;

// Use Kestrel as web server (often used with reverse proxy like Nginx)
//   Nginx is used for security, load balancing
// Configure default Configuration sources
var builder = WebApplication.CreateBuilder(args);

builder.RegisterDependencies();

var app = builder.Build();

app.ConfigureMiddleware();

// Could measure startup time by listening to the EventSource: ServerReady
app.Run();
