using CompraProgramada.Domain.PurchaseContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infrastructure.Persistence.SharedContext.Configurations;

public class BuyOrderConfiguration : IEntityTypeConfiguration<BuyOrder>
{
    public void Configure(EntityTypeBuilder<BuyOrder> builder)
    {
        builder.ToTable("BuyOrders");

        builder.HasKey(bo => bo.Id);

        builder.Property(bo => bo.MasterAccountId)
            .IsRequired();

        builder.Property(bo => bo.Ticker)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(bo => bo.Quantity)
            .IsRequired();

        builder.Property(bo => bo.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(bo => bo.MarketType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(bo => bo.ExecutionDate)
            .IsRequired();

        builder.HasIndex(bo => bo.MasterAccountId);
        builder.HasIndex(bo => bo.ExecutionDate);
    }
}
