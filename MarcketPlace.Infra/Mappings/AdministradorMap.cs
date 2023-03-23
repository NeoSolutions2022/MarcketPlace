using MarcketPlace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcketPlace.Infra.Mappings;

public class AdministradorMap : IEntityTypeConfiguration<Administrador>
{
    public void Configure(EntityTypeBuilder<Administrador> builder)
    {
        builder
            .Property(a => a.Nome)
            .IsRequired()
            .HasMaxLength(60);

        builder
            .Property(a => a.Senha)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(80);

        builder
            .Property(a => a.Desativado)
            .HasDefaultValue(false);
    }
}