using Catalog.Api.Options;
using Catalog.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// mongodb baðlantý ayarlarý
builder.Services.AddOptionsExt();
builder.Services.AddRepositoriesExt();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.Run();