using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using OrderManagement.Domain.OrderAggregate;

namespace OrderManagement.Persistence.Mongodb;

public class MongodbOrderDbContext(DbContextOptions<MongodbOrderDbContext> options) : DbContext(options)
{

    public static MongodbOrderDbContext Create(IMongoDatabase database) =>

        new(
            new DbContextOptionsBuilder<MongodbOrderDbContext>()
                .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
                .Options
        );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().ToCollection("Order");
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Order> Orders { get; set; }
}