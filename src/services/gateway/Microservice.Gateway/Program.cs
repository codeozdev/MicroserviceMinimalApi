using Shared.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Yarp confing
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

WebApplication app = builder.Build();

app.MapReverseProxy();

app.MapGet("/", () => "Yarp Gateway");


app.UseAuthentication();
app.UseAuthorization();

app.Run();