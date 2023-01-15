namespace MarcketPlace.Core.Settings;

public class EmailSettings
{
    public string Nome { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Servidor { get; set; } = string.Empty;
    public int Porta { get; set; }
}