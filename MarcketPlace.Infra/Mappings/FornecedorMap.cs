using MarcketPlace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcketPlace.Infra.Mappings;

public class FornecedorMap : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
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
            .Property(c => c.Cnpj)
            .IsRequired(false)
            .HasMaxLength(18);

        builder
            .Property(c => c.Telefone)
            .IsRequired(false)
            .HasMaxLength(17);
        
        builder
            .Property(c => c.Senha)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(c => c.CodigoResetarSenha)
            .HasColumnType("CHAR(64)")
            .HasCharSet("utf8mb4")
            .UseCollation("utf8mb4_0900_ai_ci")
            .IsRequired(false);
        
        builder
            .Property(c => c.CodigoResetarSenhaExpiraEm)
            .HasColumnType("DATETIME")
            .IsRequired(false);
    }
}