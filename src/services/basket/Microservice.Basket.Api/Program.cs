using Microservice.Basket.Api;
using Microservice.Basket.Api.Features.Baskets;
using Scalar.AspNetCore;
using Shared.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCommonServiceExt(typeof(BasketAssembly));
builder.Services.AddVersioningExt();
builder.Services.AddScoped<BasketService>();

// container redise baglant�s�
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

WebApplication app = builder.Build();


// Group endpoints
app.AddBasketGroupEndpointExt(app.AddVersionSetExt());


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();