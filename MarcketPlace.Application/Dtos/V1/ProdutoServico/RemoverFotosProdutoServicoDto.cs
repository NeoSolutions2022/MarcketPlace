namespace MarcketPlace.Application.Dtos.V1.ProdutoServico;

public class RemoverFotosProdutoServicoDto
{
    public int Id { get; set; }
    public List<Boolean> HaRemover { get; set; } = null!;
}