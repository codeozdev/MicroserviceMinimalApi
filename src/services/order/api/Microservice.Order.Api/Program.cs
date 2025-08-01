
using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Persistence;
using Microservice.Order.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});


builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();

