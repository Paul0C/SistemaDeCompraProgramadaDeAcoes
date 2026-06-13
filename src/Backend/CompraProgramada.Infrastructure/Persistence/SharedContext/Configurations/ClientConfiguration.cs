using CompraProgramada.Domain.ClientContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infrastructure.Persistence.SharedContext.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Active)
            .IsRequired();

        builder.Property(c => c.AdhesionDate)
            .IsRequired();

        builder.OwnsOne(c => c.CPF, cpf =>
        {
            cpf.Property(x => x.Number)
               .HasColumnName("Cpf")
               .IsRequired()
               .HasMaxLength(11)
               .IsFixedLength();
        });

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(x => x.Address)
                 .HasColumnName("Email")
                 .IsRequired()
                 .HasMaxLength(200);
        });

        builder.OwnsOne(c => c.MonthlyValue, mv =>
        {
            mv.Property(x => x.Value)
              .HasColumnName("MonthlyValue")
              .IsRequired()
              .HasColumnType("decimal(18,2)");
        });

        builder.HasOne(c => c.GraphicAccount)
               .WithOne()
               .HasForeignKey<GraphicAccount>(g => g.ClientId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
