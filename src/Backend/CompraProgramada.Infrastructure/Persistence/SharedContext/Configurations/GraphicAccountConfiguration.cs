using CompraProgramada.Domain.ClientContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infrastructure.Persistence.SharedContext.Configurations;

public class GraphicAccountConfiguration : IEntityTypeConfiguration<GraphicAccount>
{
    public void Configure(EntityTypeBuilder<GraphicAccount> builder)
    {
        builder.ToTable("GraphicAccounts");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.ClientId)
            .IsRequired();

        builder.HasIndex(g => g.ClientId)
            .IsUnique();

        builder.OwnsOne(g => g.AccountNumber, an =>
        {
            an.Property(x => x.Number)
              .HasColumnName("AccountNumber")
              .IsRequired()
              .HasMaxLength(30);
        });

        builder.Property(g => g.Type)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName("AccountType");

        builder.Property(g => g.CreateDate)
            .IsRequired();
    }
}
