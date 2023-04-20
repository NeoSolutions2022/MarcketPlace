using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Dtos.V1.ProdutoServico;

public class AlterarFotoProdutoServicoDto
{
    public int Id { get; set; }
    public IFormFile Foto { get; set; } = null!;
    public IFormFile? Foto2 { get; set; }
    public IFormFile? Foto3 { get; set; }
    public IFormFile? Foto4 { get; set; }
    public IFormFile? Foto5 { get; set; }
}