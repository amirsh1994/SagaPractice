using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Query.Api.ReadModel.EfModel;

namespace OrderManagement.Query.Api.Persistence.Ef;

public class OrderConfiguration:IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.CustomerName).HasMaxLength(10);
        builder.Property(x => x.NationalCode).HasMaxLength(20);
    }
}

public class InBoxMessageConfiguration:IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.HasKey(x => x.MessageId);
    }
}