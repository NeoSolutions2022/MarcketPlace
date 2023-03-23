using FluentEmail.MailKitSmtp;
using MarcketPlace.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarcketPlace.Application.Configuration.DependencyInjection;

public static class EmailFactoryConfiguration
{
    public static void AddEmailFactory(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblyPath = Path.GetDirectoryName(typeof(Application.DependencyInjection).Assembly.Location);

        var emailSettings = configuration.GetSection("EmailSettings");
        services
            .AddFluentEmail(emailSettings.GetValue<string>("Usuario").FromBase64())
            .AddRazorRenderer(Path.Combine(assemblyPath!, "Email/Templates"))
            .AddMailKitSender(new SmtpClientOptions
            {
                User = emailSettings.GetValue<string>("Usuario").FromBase64(),
                Password = emailSettings.GetValue<string>("Senha").FromBase64(),
                Server = emailSettings.GetValue<string>("Servidor"),
                Port = emailSettings.GetValue<int>("Porta"),
                RequiresAuthentication = true
            });
    }
}