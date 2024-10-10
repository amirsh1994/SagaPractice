using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace OrderManagement.Persistence.Mongodb;

public static class OrderPersistenceMongodbBootstrap
{
    public static void AddMongodbPersistenceResolver(this IServiceCollection service,IConfiguration configuration)
    {

        service.AddSingleton<MongoClient>(option => new MongoClient(configuration.GetConnectionString("MongodbConnection")));

        service.AddSingleton(sp =>
        {
            var mongodbClient = sp.GetRequiredService<MongoClient>();
            var databaseName = configuration["ConnectionStrings:MongodbDatabase"];
            return mongodbClient.GetDatabase(databaseName);

        });

        service.AddDbContext<MongodbOrderDbContext>(option =>
        {
            var client = service.BuildServiceProvider().GetService<MongoClient>();
            var databaseName = configuration["ConnectionStrings:MongodbDatabase"];
            option.UseMongoDB(client, databaseName);

        });

    }
}