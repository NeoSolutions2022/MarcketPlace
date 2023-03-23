using FluentEmail.Core;
using MarcketPlace.Application.BackgroundJob;
using MarcketPlace.Core.Extensions;
using MarcketPlace.Core.Settings;
using Microsoft.Extensions.Options;

namespace MarcketPlace.Application.Email;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IBackgroundClient _backgroundClient;
    private readonly EmailSettings _emailSettings;

    public EmailService(IFluentEmail fluentEmail, IBackgroundClient backgroundClient, IOptions<EmailSettings> emailSettings)
    {
        _fluentEmail = fluentEmail;
        _backgroundClient = backgroundClient;
        _emailSettings = emailSettings.Value;
    }

    public async Task EnviarAsync(string destinatario, string assunto, string template, object model)
    {
        await EnviarEmail(destinatario, assunto, MountPath(template), model);
    }
    
    public void Enviar(string destinatario, string assunto, string template, object model, TimeSpan? delay = null)
    {
        var templatePath = MountPath(template);
        
        _backgroundClient
            .Schedule(() => EnviarEmail(destinatario, assunto, templatePath, model), delay ?? TimeSpan.Zero);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task EnviarEmail(string destinatario, string assunto, string template, object model)
    {
        await _fluentEmail
            .SetFrom(_emailSettings.Usuario.FromBase64(), _emailSettings.Nome)
            .To(destinatario)
            .Subject(assunto)
            .UsingTemplateFromFile(template, model)
            .SendAsync();
    }
    
    private static string MountPath(string templateName)
    {
        const string fileFormat = ".cshtml";
        
        var assemblyPath = Path.GetDirectoryName(typeof(DependencyInjection).Assembly.Location);
        var path = Path.Combine(assemblyPath!, "Email/Templates");

        if (!templateName.EndsWith(fileFormat))
        {
            templateName += fileFormat;
        }
        
        return Path.Combine(path, templateName);
    }
}