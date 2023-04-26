using MarcketPlace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcketPlace.Infra.Mappings;

public class ProdutoServicoCaracteristicaMap : IEntityTypeConfiguration<ProdutoServicoCaracteristica>
{
    public void Configure(EntityTypeBuilder<ProdutoServicoCaracteristica> builder)
    {
        builder
            .Property(c => c.Chave)
            .HasMaxLength(100);

        builder
            .Property(c => c.Valor)
            .HasMaxLength(255);
    }
}