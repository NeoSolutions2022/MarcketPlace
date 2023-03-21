namespace MarcketPlace.Application.Dtos.V1.ProdutoServico;

public class ProdutoServicoDto
{
    public List<string> Fotos { get; set; } = new();
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public bool Desativado { get; set; }
}