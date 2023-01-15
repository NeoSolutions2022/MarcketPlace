using System.Linq.Expressions;

namespace MarcketPlace.Domain.Contracts.Paginacao;

public interface IBuscaPaginada<T> where T : IEntity
{
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public string OrdenarPor { get; set; }
    public string DirecaoOrdenacao { get; set; }

    public void AplicarFiltro(ref IQueryable<T> query);
    public void AplicarOrdenacao(ref IQueryable<T> query);
    public Expression<Func<T, bool>> MontarExpressao();
}