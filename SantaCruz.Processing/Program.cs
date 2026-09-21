using SantaCruz.Processing;
using SantaCruz.Application.DI;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.Configure<ApiConfig>(builder.Configuration.GetSection("ApiConfig"));
builder.Services.AddApplication();


var host = builder.Build();
host.Run();
