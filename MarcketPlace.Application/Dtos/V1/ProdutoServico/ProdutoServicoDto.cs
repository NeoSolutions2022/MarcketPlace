using MarcketPlace.Application.Dtos.V1.Fornecedor;
using MarcketPlace.Application.Dtos.V1.ProdutoServico.ProdutoServicoCaracteristica;

namespace MarcketPlace.Application.Dtos.V1.ProdutoServico;

public class ProdutoServicoDto
{
    public int Id { get; set; }
    public string Foto { get; set; } = null!;
    public string? Foto2 { get; set; }
    public string? Foto3 { get; set; }
    public string? Foto4 { get; set; }
    public string? Foto5 { get; set; }
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public double Preco { get; set; }
    public double PrecoDesconto { get; set; }
    public bool Desativado { get; set; }
    public int FornecedorId { get; set; }
    public string Categoria { get; set; } = null!;
    public string? Caracteristica { get; set; }
    public FornecedorDto? Fornecedor { get; set; }
    public List<ProdutoServicoCaracteristicaDto> ProdutoServicoCaracteristicas { get; set; } = new();
}