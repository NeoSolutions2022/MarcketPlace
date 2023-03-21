using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Dtos.V1.ProdutoServico;

public class CadastrarProdutoServicoDto
{
    public List<IFormFile> Fotos { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public bool Desativado { get; set; }
}