using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Dtos.V1.ProdutoServico;

public class AlterarFotoProdutoServicoDto
{
    public int Id { get; set; }
    public List<Boolean> HaAlterar { get; set; } = null!;
    public List<IFormFile> Fotos { get; set; } = null!;
}