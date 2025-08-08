using Microservice.Payment.Api;
using Microservice.Payment.Api.Features.Payments;
using Microservice.Payment.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Shared.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddVersioningExt();
builder.Services.AddCommonServiceExt(typeof(PaymentAssembly));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("payment-in-memory-db");
});

// bu microservice bizim yazdigimiz kimlik dogrulama ve yetkilendirme servisini ekler
builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

WebApplication app = builder.Build();

// Group endpoints
app.AddPaymentGroupEndpointExt(app.AddVersionSetExt());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

// siralamasi onemli (middleware)
app.UseAuthentication();
app.UseAuthorization();


app.Run();