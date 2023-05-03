using MarcketPlace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcketPlace.Infra.Mappings;

public class ProdutoServicoMap : IEntityTypeConfiguration<ProdutoServico>
{
    public void Configure(EntityTypeBuilder<ProdutoServico> builder)
    {
        builder.Property(c => c.Titulo)
            .HasMaxLength(180)
            .IsRequired();

        builder.Property(c => c.Descricao)
            .IsRequired()
            .HasMaxLength(1500);

        builder.Property(c => c.Foto)
            .IsRequired(false)
            .HasMaxLength(1500);

        builder.Property(c => c.Foto2)
            .IsRequired(false)
            .HasMaxLength(1500);

        builder.Property(c => c.Foto3)
            .IsRequired(false)
            .HasMaxLength(1500);

        builder.Property(c => c.Foto4)
            .IsRequired(false)
            .HasMaxLength(1500);

        builder.Property(c => c.Foto5)
            .IsRequired(false)
            .HasMaxLength(1500);

        builder.Property(c => c.Preco)
            .IsRequired();

        builder.Property(c => c.PrecoDesconto)
            .IsRequired();
        
        builder.Property(c => c.AnuncioPago)
            .HasDefaultValue(false);

        builder
            .Property(c => c.Categoria)
            .IsRequired();

        builder.Property(c => c.Caracteristica)
            .HasMaxLength(8000);
    }
}