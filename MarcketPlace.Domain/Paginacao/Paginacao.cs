using MarcketPlace.Domain.Contracts.Paginacao;

namespace MarcketPlace.Domain.Paginacao;

public class Paginacao : IPaginacao
{
    public int Total { get; set; }
    public int TotalNaPaginacao { get; set; }
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalDePaginas { get; set; }
}