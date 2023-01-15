namespace MarcketPlace.Domain.Contracts;

public interface ITracking
{
    public DateTime CriadoEm { get; set; }
    public int? CriadoPor { get; set; }
    public bool CriadoPorAdmin { get; set; }

    public DateTime AtualizadoEm { get; set; }
    public int? AtualizadoPor { get; set; }
    public bool AtualizadoPorAdmin { get; set; }
}