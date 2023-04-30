using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Dtos.V1.ProdutoServico;

public class CadastrarProdutoServicoDto
{
    public IFormFile Foto { get; set; } = null!;
    public IFormFile? Foto2 { get; set; }
    public IFormFile? Foto3 { get; set; }
    public IFormFile? Foto4 { get; set; }
    public IFormFile? Foto5 { get; set; }
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public double Preco { get; set; }
    public double PrecoDesconto { get; set; }
    public bool Desativado { get; set; }
    public string Categoria { get; set; } = null!;
    public string? Caracteristica { get; set; }
}