using Microsoft.EntityFrameworkCore;
using OrderManagement.Query.Api.ReadModel.EfModel;

namespace OrderManagement.Query.Api.Persistence.Ef;

public class OrderManagementQueryDbContext(DbContextOptions<OrderManagementQueryDbContext> options):DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> Items { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InBoxMessageConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}