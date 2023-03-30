namespace MarcketPlace.Application.Email;

public interface IEmailService
{
    Task EnviarAsync(string destinatario, string assunto, string template, object model);
    void Enviar(string destinatario, string assunto, string template, object model, TimeSpan? delay = null);
}