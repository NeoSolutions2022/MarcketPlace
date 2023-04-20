using MarcketPlace.Application.Dtos.V1.Fornecedor;

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
    public bool Desativado { get; set; }
    public int FornecedorId { get; set; }
    public string Categoria { get; set; } = null!;
    public FornecedorDto? Fornecedor { get; set; }
}