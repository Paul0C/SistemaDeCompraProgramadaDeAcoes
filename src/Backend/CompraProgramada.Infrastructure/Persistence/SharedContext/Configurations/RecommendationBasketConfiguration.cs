using CompraProgramada.Domain.RecommendationBasketContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infrastructure.Persistence.SharedContext.Configurations;

public class RecommendationBasketConfiguration : IEntityTypeConfiguration<RecommendationBasket>
{
    public void Configure(EntityTypeBuilder<RecommendationBasket> builder)
    {
        builder.ToTable("RecommendationBaskets");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Active)
            .IsRequired();

        builder.Property(b => b.CreateDate)
            .IsRequired();

        builder.Property(b => b.DeactivationDate);

        builder.HasMany(b => b.BasketItems)
               .WithOne()
               .HasForeignKey(bi => bi.RecommendationBasketId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(b => b.BasketItems)
               .HasField("_basketItems")
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
