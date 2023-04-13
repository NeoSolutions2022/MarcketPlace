namespace MarcketPlace.Domain.Contracts.Repositories;

public interface IAnunciavel
{
    public bool AnuncioPago { get; set; }
    public DateTime? DataPagamentoAnuncio { get; set; }
    public DateTime? DataExpiracaoAnuncio { get; set; }
}