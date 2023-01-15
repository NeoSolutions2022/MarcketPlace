using MarcketPlace.Domain.Contracts;
using MarcketPlace.Domain.Contracts.Paginacao;

namespace MarcketPlace.Application.Dtos.V1.Base;

public class PagedDto<T> : IResultadoPaginado<T>, IViewModel
{
    public IList<T> Itens { get; set; }
    public IPaginacao Paginacao { get; set; }

    public PagedDto()
    {
        Itens = new List<T>();
        Paginacao = new PaginacaoDto();
    }
}