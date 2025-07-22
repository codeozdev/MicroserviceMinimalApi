using Catalog.Api;
using Catalog.Api.Features.Categories;
using Catalog.Api.Features.Courses;
using Catalog.Api.Options;
using Catalog.Api.Repositories;
using Scalar.AspNetCore;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// mongodb baðlantý ayarlarý
builder.Services.AddOptionsExt();
builder.Services.AddRepositoriesExt();


// FluentValidation and MediatR ayrica Filterin devreye girmesini saglar
builder.Services.AddCommonServiceExt(typeof(CatelogAssembly));


// Versiyonlama icin -> Shared/VersioningExt
builder.Services.AddVersioningExt();


var app = builder.Build();

// Seed data -> continuewith ile olumlu veya olumsuz sonuclari yakalayabiliriz bu yuzden kullandik
app.AddSeedDataExt().ContinueWith(x =>
{
    Console.WriteLine(x.IsFaulted ? x.Exception?.Message : "Seed data has been saved successfully");
});


// Group endpoints
app.AddCategoryGroupEndpointExt(app.AddVersionSetExt());
app.AddCourseGroupEndpointExt(app.AddVersionSetExt());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}


app.Run();