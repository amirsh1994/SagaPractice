using InventoryManagement.Domain.StockAggregate;
using InventoryManagement.Persistence.Mongodb.MongodbInBoxModel;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace InventoryManagement.Persistence.Mongodb;

public class MongodbContextInventory(DbContextOptions<MongodbContextInventory> options):DbContext(options)
{

    public static MongodbContextInventory Create(IMongoDatabase database)
        => new(
            new DbContextOptionsBuilder<MongodbContextInventory>()
                .UseMongoDB(database.Client,database.DatabaseNamespace.DatabaseName)
                .Options
            );


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InBoxMessageFromMongodb>().ToCollection("InboxMessages");
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<InBoxMessageFromMongodb> Messages { get; set; }
}