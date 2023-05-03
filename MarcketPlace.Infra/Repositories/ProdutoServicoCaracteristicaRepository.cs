using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using MarcketPlace.Infra.Abstractions;
using MarcketPlace.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MarcketPlace.Infra.Repositories;

public class ProdutoServicoCaracteristicaRepository : Repository<ProdutoServicoCaracteristica>,
    IProdutoServicoCaracteristicaRepository
{
    public ProdutoServicoCaracteristicaRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public void Adicionar(List<ProdutoServicoCaracteristica> produtoServicoCaracteristica)
    {
        Context.ProdutoServicoCaracteristicas.AddRange(produtoServicoCaracteristica);
    }

    public void Alterar(ProdutoServicoCaracteristica produtoServicoCaracteristica)
    {
        Context.ProdutoServicoCaracteristicas.UpdateRange(produtoServicoCaracteristica);
    }

    public async Task<ProdutoServicoCaracteristica?> ObterPorId(int id)
    {
        return await Context.ProdutoServicoCaracteristicas
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<ProdutoServicoCaracteristica>?> ObterTodos(int id)
    {
        return await Context.ProdutoServicoCaracteristicas.Where(c => c.ProdutoServico.Id == id)
            .ToListAsync();
    }

    public void Remover(ProdutoServicoCaracteristica produtoServicoCaracteristica)
    {
        Context.ProdutoServicoCaracteristicas.RemoveRange(produtoServicoCaracteristica);
    }
}