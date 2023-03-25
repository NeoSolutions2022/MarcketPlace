using MarcketPlace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcketPlace.Infra.Mappings;

public class ClienteMap : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder
            .Property(c => c.Cep)
            .IsRequired()
            .HasMaxLength(9);
        
        builder
            .Property(c => c.Cidade)
            .IsRequired()
            .HasMaxLength(60);
        
        builder
            .Property(c => c.Complemento)
            .IsRequired(false)
            .HasMaxLength(60);
        
        builder
            .Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(14);
        
        builder
            .Property(c => c.DataPagamento)
            .IsRequired();
        
        builder
            .Property(c => c.Desativado)
            .HasDefaultValue(false)
            .IsRequired();
        
        builder
            .Property(c => c.Email)
            .HasMaxLength(60)
            .IsRequired();
        
        builder
            .Property(c => c.Endereco)
            .HasMaxLength(60)
            .IsRequired();
        
        builder
            .Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(60);
        
        builder
            .Property(c => c.NomeSocial)
            .IsRequired(false)
            .HasMaxLength(60);
        
        builder
            .Property(c => c.Numero)
            .IsRequired();
        
        builder
            .Property(c => c.Senha)
            .IsRequired()
            .HasMaxLength(255);
        
        builder
            .Property(c => c.Telefone)
            .IsRequired(false)
            .HasMaxLength(17);
        
        builder
            .Property(c => c.Uf)
            .IsRequired()
            .HasMaxLength(2);
        
        builder
            .Property(c => c.Inadiplente)
            .IsRequired(false)
            .HasDefaultValue(false);
        
        builder
            .Property(c => c.CodigoResetarSenha)
            .HasColumnType("CHAR(64)")
            .IsRequired(false);
        
        builder
            .Property(c => c.CodigoResetarSenhaExpiraEm)
            .HasColumnType("DATETIME")
            .IsRequired(false);
    }
}