using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.OrderAggregate;

namespace OrderManagement.Persistence.Ef.OrderMapping;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();
        builder.HasKey(x => x.Id);
        builder.OwnsMany(x => x.Items, option =>
        {
            option.HasKey(x => x.Id);
            option.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            option.ToTable("Items", "Order");
            option.OwnsOne(x => x.UnitPrice, cfg =>
            {
                cfg.Property(x => x.Value).HasColumnName("Value");
            });
        });
        builder.Metadata.FindNavigation(nameof(Order.Items))?.SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Property(x => x.State).HasConversion
        (
            x => x.GetType().Name,
            x =>GetOrderState(x)

        ).HasMaxLength(20);

    }

    private OrderState GetOrderState(string s)
    {
        return s switch
        {
            $"{nameof(PendingState)}" => new PendingState(),
            $"{nameof(ApprovedState)}" => new ApprovedState(),
            $"{nameof(RejectedState)}" => new RejectedState(),
            _=>throw new NotImplementedException()
        };
    }
}