using MarcketPlace.Domain.Contracts.Paginacao;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Contracts.Repositories;

public interface IProdutoServicoRepository : IRepository<ProdutoServico>
{
    void Adicionar(ProdutoServico produtoServico);
    void Alterar(ProdutoServico produtoServico);
    Task<ProdutoServico?> ObterPorId(int id);
    Task<IResultadoPaginado<ProdutoServico>> Buscar(IBuscaPaginada<ProdutoServico> filtro);
    void Remover(ProdutoServico produtoServico);
}