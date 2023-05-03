using System.Reflection;
using MarcketPlace.Application.BackgroundJob;
using MarcketPlace.Application.Configuration.DependencyInjection;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Email;
using MarcketPlace.Application.Notification;
using MarcketPlace.Application.Services;
using MarcketPlace.Core.Settings;
using MarcketPlace.Domain.Entities;
using MarcketPlace.Infra;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScottBrady91.AspNetCore.Identity;

namespace MarcketPlace.Application;

public static class DependencyInjection
{
    public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<UploadSettings>(configuration.GetSection("UploadSettings"));

        services.ConfigureDataBase(configuration);
        services.ConfigureRepositories();

        services
            .AddEmailFactory(configuration);
        services
            .AddHangfireConfig(configuration);
        services
            .AddAutoMapper(Assembly.GetExecutingAssembly());
    }
    
    public static void ConfigureServices(this IServiceCollection services)
    {
        services
            .AddScoped<IClienteService, ClienteService>()
            .AddScoped<IFornecedorService, FornecedorService>()
            .AddScoped<IBackgroundClient, BackgroundClient>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IAdministradorService, AdministradorService>();

        services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IProdutoServicoService, ProdutoServicoService>()
            .AddScoped<IPagamentosService, PagamentosService>()
            .AddScoped<IProdutoServicoCaracteristicaService, ProdutoServicoCaracteristicaService>()
            .AddScoped<IFileService, FileService>()
            .AddScoped<IFornecedorAuthService, FornecedorAuthService>()
            .AddScoped<IClienteAuthService, ClienteAuthService>()
            .AddScoped<INotificator, Notificator>()
            .AddScoped<IPasswordHasher<Cliente>, Argon2PasswordHasher<Cliente>>()
            .AddScoped<IPasswordHasher<Fornecedor>, Argon2PasswordHasher<Fornecedor>>()
            .AddScoped<IPasswordHasher<Administrador>, Argon2PasswordHasher<Administrador>>();
    }
    
    // public static void UseStaticFileConfiguration(this IApplicationBuilder app, IConfiguration configuration)
    // {
    //     var uploadSettings = configuration.GetSection("UploadSettings");
    //     var publicBasePath = uploadSettings.GetValue<string>("PublicBasePath");
    //     var privateBasePath = uploadSettings.GetValue<string>("PrivateBasePath");
    //
    //     app.UseStaticFiles(new StaticFileOptions
    //     {
    //         FileProvider = new PhysicalFileProvider(publicBasePath),
    //         RequestPath = $"/{EPathAccess.Public.ToDescriptionString()}"
    //     });
    //
    //     app.UseStaticFiles(new StaticFileOptions
    //     {
    //         FileProvider = new PhysicalFileProvider(privateBasePath),
    //         RequestPath = $"/{EPathAccess.Private.ToDescriptionString()}",
    //         OnPrepareResponse = ctx =>
    //         {
    //             if (ctx.Context.User.UsuarioAutenticado()) return;
    //
    //             // respond HTTP 401 Unauthorized.
    //             ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    //             ctx.Context.Response.ContentLength = 0;
    //             ctx.Context.Response.Body = Stream.Null;
    //             ctx.Context.Response.Headers.Add("Cache-Control", "no-store");
    //         }
    //     });
    // }
}