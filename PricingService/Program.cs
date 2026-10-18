using Steeltoe.Discovery.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDiscoveryClient(builder.Configuration);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
