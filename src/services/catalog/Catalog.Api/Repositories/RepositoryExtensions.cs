using Catalog.Api.Options;
using MongoDB.Driver;

namespace Catalog.Api.Repositories;

public static class RepositoryExtensions
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