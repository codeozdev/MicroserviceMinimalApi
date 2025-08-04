WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Yarp confing
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

WebApplication app = builder.Build();

app.MapReverseProxy();

app.MapGet("/", () => "Yarp Gateway");

app.Run();