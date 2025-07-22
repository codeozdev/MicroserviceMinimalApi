using Discount.Api;
using Scalar.AspNetCore;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Shared -> FluentValidation and MediatR ayrica Filterin devreye girmesini saglar
builder.Services.AddCommonServiceExt(typeof(DiscountAssembly));

// Versiyonlama icin -> Shared/VersioningExt
builder.Services.AddVersioningExt();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}



app.Run();



