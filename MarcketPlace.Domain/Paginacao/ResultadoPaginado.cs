using MarcketPlace.Domain.Contracts.Paginacao;

namespace MarcketPlace.Domain.Paginacao;

public class ResultadoPaginado<T> : IResultadoPaginado<T>
{
    public IList<T> Itens { get; set; }
    public IPaginacao Paginacao { get; set; }

    public ResultadoPaginado()
    {
        Itens = new List<T>();
        Paginacao = new Paginacao();
    }

    public ResultadoPaginado(int pagina, int tamanhoPagina, int total) : this()
    {
        Paginacao = new Paginacao
        {
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            Total = total
        };
    }
}