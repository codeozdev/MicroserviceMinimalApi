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



var app = builder.Build();

// Group endpoints
app.AddCategoryGroupEndpointExt();
app.AddCourseGroupEndpointExt();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}


app.Run();