using System.Reflection;
using FluentValidation.Results;
using MarcketPlace.Core.Authorization;
using MarcketPlace.Domain.Contracts;
using MarcketPlace.Domain.Entities;
using MarcketPlace.Infra.Converters;
using MarcketPlace.Infra.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MarcketPlace.Infra.Context;

public abstract class BaseApplicationDbContext : DbContext, IUnitOfWork
{
    protected readonly IAuthenticatedUser AuthenticatedUser;

    protected BaseApplicationDbContext(DbContextOptions options ,IAuthenticatedUser authenticatedUser) : base(options)
    {
        AuthenticatedUser = authenticatedUser;
    }

    public DbSet<Administrador> Administradores { get; set; } = null!;
    public DbSet<Cliente> Clientes { get; set; } = null!;
    public DbSet<Fornecedor> Fornecedores { get; set; } = null!;
    public DbSet<ProdutoServico> ProdutoServicos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        ApplyConfiguration(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (AuthenticatedUser.UsuarioAdministrador)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> Commit() => await SaveChangesAsync() > 0;

        private static void ApplyConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.ApplyEntityConfiguration();
        // modelBuilder.ApplyTrackingConfiguration();
        modelBuilder.ApplySoftDeleteConfiguration();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateOnly>()
            .HaveConversion<DateOnlyCustomConverter>()
            .HaveColumnType("DATE");

        configurationBuilder 
            .Properties<TimeOnly>()
            .HaveConversion<TimeOnlyCustomConverter>()
            .HaveColumnType("TIME");
    }
}