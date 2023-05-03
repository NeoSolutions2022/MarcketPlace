using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Contracts.Repositories;

public interface IProdutoServicoCaracteristicaRepository : IRepository<ProdutoServicoCaracteristica>
{
    void Adicionar(List<ProdutoServicoCaracteristica> produtoServicoCaracteristica);
    void Alterar(ProdutoServicoCaracteristica produtoServicoCaracteristica);
    Task<ProdutoServicoCaracteristica?> ObterPorId(int id);
    Task<List<ProdutoServicoCaracteristica>?> ObterTodos(int id);
    void Remover(ProdutoServicoCaracteristica produtoServicoCaracteristica);
}