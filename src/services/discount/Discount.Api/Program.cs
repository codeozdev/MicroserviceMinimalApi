using Discount.Api;
using Discount.Api.Features.Discounts;
using Discount.Api.Options;
using Discount.Api.Repositories;
using Scalar.AspNetCore;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// mongodb baðlantý ayarlarý
builder.Services.AddOptionsExt();
builder.Services.AddRepositoriesExt();


// Shared -> FluentValidation and MediatR ayrica Filterin devreye girmesini saglar
builder.Services.AddCommonServiceExt(typeof(DiscountAssembly));

// Versiyonlama icin -> Shared/VersioningExt
builder.Services.AddVersioningExt();

var app = builder.Build();

// Group endpoints
app.AddDiscountGroupEndpointExt(app.AddVersionSetExt());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}



app.Run();



// discount endpointlerini yonetim belirler ve uygular