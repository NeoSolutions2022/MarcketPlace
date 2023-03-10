using MarcketPlace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcketPlace.Infra.Mappings;

public class ClienteMap : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder
            .Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(60);
        
        builder
            .Property(c => c.NomeSocial)
            .IsRequired(false)
            .HasMaxLength(60);
        
        builder
            .Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(80);
        
        builder
            .Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(14);
        
        builder
            .Property(c => c.Telefone)
            .IsRequired(false)
            .HasMaxLength(17);
        
        builder
            .Property(c => c.Senha)
            .IsRequired()
            .HasMaxLength(255);
        
        builder
            .Property(c => c.Inadiplente)
            .IsRequired(false)
            .HasDefaultValue(false);
        
        builder
            .Property(c => c.DataPagamento)
            .IsRequired();
    }
}