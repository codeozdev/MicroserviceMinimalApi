using Discount.Api.Options;
using MongoDB.Driver;

namespace Discount.Api.Repositories;

public static class RepositoryExt
{
    public static IServiceCollection AddRepositoriesExt(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient, MongoClient>(sp =>
        {
            var options = sp.GetRequiredService<MongoOption>();
            return new MongoClient(options.ConnectionString);
        });

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var options = sp.GetRequiredService<MongoOption>();
            var database = client.GetDatabase(options.DatabaseName);
            return AppDbContext.Create(database);
        });

        return services;
    }
}