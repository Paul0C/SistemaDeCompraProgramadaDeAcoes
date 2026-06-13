using CompraProgramada.Domain.CustodyContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infrastructure.Persistence.SharedContext.Configurations;

public class CustodyConfiguration : IEntityTypeConfiguration<Custody>
{
    public void Configure(EntityTypeBuilder<Custody> builder)
    {
        builder.ToTable("Custodies");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.GraphicAccountId)
            .IsRequired();

        builder.Property(c => c.Ticker)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Quantity)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.AveragePrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.LastUpdateDate)
            .IsRequired();

        builder.HasMany(c => c.Distributions)
               .WithOne()
               .HasForeignKey(d => d.BabyCustodyId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(c => c.Distributions)
               .HasField("_distributions")
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
