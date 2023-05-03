using MarcketPlace.Core.Authorization;
using MarcketPlace.Core.Extensions;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Infra.Context;
using MarcketPlace.Infra.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarcketPlace.Infra;

public static class DependencyInjection
{
    public static void ConfigureDataBase(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddHttpContextAccessor();

        service.AddScoped<IAuthenticatedUser>(sp =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

            return httpContextAccessor.UsuarioAutenticado()
                ? new AuthenticatedUser(httpContextAccessor)
                : new AuthenticatedUser();
        });

        service.AddDbContext<ApplicationDbContext>(optionsAction =>
        {
            optionsAction.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            optionsAction.EnableDetailedErrors();
            optionsAction.EnableSensitiveDataLogging();
        });
        
        service.AddScoped<BaseApplicationDbContext>(serviceProvider =>
        {
            return serviceProvider.GetRequiredService<ApplicationDbContext>();
        });
    }

    public static void ConfigureRepositories(this IServiceCollection service)
    {
        service
            .AddScoped<IClienteRepository,ClienteRepository>()
            .AddScoped<IFornecedorRepository, FornecedorRepository>()
            .AddScoped<IAdministradorRepository, AdministradorRepository>()
            .AddScoped<IProdutoServicoCaracteristicaRepository, ProdutoServicoCaracteristicaRepository>()
            .AddScoped<IProdutoServicoRepository, ProdutoServicoRepository>();
    }

    public static void UseMigrations(this IApplicationBuilder app, IServiceProvider service)
    {
        using var scope = service.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
}