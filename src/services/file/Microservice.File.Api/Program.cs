using Microservice.File.Api;
using Microservice.File.Api.Features.Files;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExt(typeof(FileAssembly));
builder.Services.AddVersioningExt();
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

var app = builder.Build();


// Group endpoints
app.AddFileGroupEndpointExt(app.AddVersionSetExt());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseStaticFiles(); // wwwroot klasorunu dis dunyadan eriþilebilir hale getirir


app.Run();

