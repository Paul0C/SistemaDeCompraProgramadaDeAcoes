using CompraProgramada.Domain.RecommendationBasketContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infrastructure.Persistence.SharedContext.Configurations;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.ToTable("BasketItems");

        builder.HasKey(bi => bi.Id);

        builder.Property(bi => bi.RecommendationBasketId)
            .IsRequired();

        builder.Property(bi => bi.Ticker)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(bi => bi.Percentage)
            .IsRequired()
            .HasColumnType("decimal(3,2)");
    }
}
