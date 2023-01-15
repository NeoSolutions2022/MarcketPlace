namespace MarcketPlace.Core.Settings;

public class JwtSettings
{
    public int ExpiracaoHoras { get; set; }
    public string Emissor { get; set; } = string.Empty;
    public string ComumValidoEm { get; set; } = string.Empty;
    public string GestaoValidoEm { get; set; } = string.Empty;
    public string CaminhoKeys { get; set; } = string.Empty;

    public List<string> Audiences()
    {
        return new List<string> { ComumValidoEm, GestaoValidoEm };
    }
}