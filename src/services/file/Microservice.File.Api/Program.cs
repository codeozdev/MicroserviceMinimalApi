using Microservice.File.Api;
using Microservice.File.Api.Features.Files;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;
using Shared.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExt(typeof(FileAssembly));
builder.Services.AddVersioningExt();
builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

WebApplication app = builder.Build();


// Group endpoints
app.AddFileGroupEndpointExt(app.AddVersionSetExt());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseStaticFiles(); // wwwroot klasorunu dis dunyadan eri�ilebilir hale getirir

app.UseAuthentication();
app.UseAuthorization();

app.Run();