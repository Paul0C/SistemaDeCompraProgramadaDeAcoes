using CompraProgramada.Domain.CustodyContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infrastructure.Persistence.SharedContext.Configurations;

public class DistributionConfiguration : IEntityTypeConfiguration<Distribution>
{
    public void Configure(EntityTypeBuilder<Distribution> builder)
    {
        builder.ToTable("Distributions");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.BabyCustodyId)
            .IsRequired();

        builder.Property(d => d.BuyOrderId)
            .IsRequired();

        builder.HasIndex(d => d.BuyOrderId);

        builder.Property(d => d.Ticker)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.Quantity)
            .IsRequired();

        builder.Property(d => d.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(d => d.DistributionDate)
            .IsRequired();
    }
}
