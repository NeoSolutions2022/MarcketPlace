namespace MarcketPlace.Application.Dtos.V1.ProdutoServico;

public class AlterarProdutoServicoDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public double Preco { get; set; }
    public double PrecoDesconto { get; set; }
    public string Descricao { get; set; } = null!;
    public bool Desativado { get; set; }
    public string Categoria { get; set; } = null!;
}