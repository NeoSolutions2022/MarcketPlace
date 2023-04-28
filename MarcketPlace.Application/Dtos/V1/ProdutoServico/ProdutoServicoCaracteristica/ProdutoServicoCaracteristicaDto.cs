namespace MarcketPlace.Application.Dtos.V1.ProdutoServico.ProdutoServicoCaracteristica;

public class ProdutoServicoCaracteristicaDto
{
    public int Id { get; set; }
    public string? Chave { get; set; }
    public string? Valor { get; set; }
    public int ProdutoServicoId { get; set; }
}