namespace MarcketPlace.Application.Dtos.V1.ProdutoServico.ProdutoServicoCaracteristica;

public class AdicionarProdutoServicoCaracteristicaDto
{
    public string? Chave { get; set; }
    public string? Valor { get; set; }
    public int ProdutoServicoId { get; set; }
}