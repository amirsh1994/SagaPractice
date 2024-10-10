using InventoryManagement.Domain.StockAggregate;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Persistence.Ef;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<InBoxMessage> Messages { get; set; }
    public DbSet<Stock> Stocks { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}