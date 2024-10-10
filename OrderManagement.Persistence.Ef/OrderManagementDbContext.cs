using Framework.Persistence.Ef;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.OrderAggregate;

namespace OrderManagement.Persistence.Ef;

public class OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options):ApplicationDbContext(options)
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}