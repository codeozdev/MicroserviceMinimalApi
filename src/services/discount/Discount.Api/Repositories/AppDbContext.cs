using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Reflection;

namespace Discount.Api.Repositories;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Features.Discounts.Discount> Discounts { get; set; } = null!;

    // Mongodb'de bir migration işlemi olmadığı için bağlanırken bu şekilde bir metot kullanıyoruz.
    public static AppDbContext Create(IMongoDatabase database)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>().UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
        return new AppDbContext(optionsBuilder.Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}