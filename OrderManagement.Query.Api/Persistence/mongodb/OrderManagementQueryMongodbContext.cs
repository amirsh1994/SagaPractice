using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using OrderManagement.Query.Api.ReadModel.EfModel;
using OrderManagement.Query.Api.ReadModel.MongodbModels;

namespace OrderManagement.Query.Api.Persistence.mongodb;

public class OrderManagementQueryMongodbContext(DbContextOptions<OrderManagementQueryMongodbContext>options):DbContext(options)
{

    public static OrderManagementQueryMongodbContext Create(IMongoDatabase database) =>
        new(
            new  DbContextOptionsBuilder<OrderManagementQueryMongodbContext>()
            .UseMongoDB(database.Client,database.DatabaseNamespace.DatabaseName)
            .Options
            );

    public DbSet<OrderMongodb> Orders { get; set; }
    //public DbSet<OrderItemMongodb> Items { get; set; }
    public DbSet<InboxMessageMongodb> Messages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderMongodb>().ToCollection("Order");
        //modelBuilder.Entity<OrderItemMongodb>().ToCollection("Items");
        modelBuilder.Entity<InboxMessageMongodb>().ToCollection("Messages");

        //modelBuilder.Entity<Order>()
        //    .Property(p => p.OrderId)
        //    .HasConversion(new StringToGuidConverter());

        //modelBuilder.Entity<Order>()
        //    .Property(p => p.CustomerId)
        //    .HasConversion(new StringToGuidConverter());

        //modelBuilder.Entity<OrderItem>()
        //    .Property(p => p.OrderItemId)
        //    .HasConversion(new StringToGuidConverter());

        //modelBuilder.Entity<OrderItem>()
        //    .Property(p => p.OrderId)
        //    .HasConversion(new StringToGuidConverter());

        //modelBuilder.Entity<OrderItem>()
        //    .Property(p => p.ProductId)
        //    .HasConversion(new StringToGuidConverter());

        //modelBuilder.Entity<InboxMessage>()
        //    .Property(p => p.MessageId)
        //    .HasConversion(new StringToGuidConverter());

        base.OnModelCreating(modelBuilder);
    }
}