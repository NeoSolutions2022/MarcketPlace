using MarcketPlace.Domain.Contracts.Paginacao;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using MarcketPlace.Infra.Abstractions;
using MarcketPlace.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MarcketPlace.Infra.Repositories;

public class ProdutoServicoRepository : Repository<ProdutoServico>, IProdutoServicoRepository
{
    public ProdutoServicoRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public void Adicionar(ProdutoServico produtoServico)
    {
        Context.ProdutoServicos.Add(produtoServico);
    }

    public void Alterar(ProdutoServico produtoServico)
    {
        Context.ProdutoServicos.Update(produtoServico);
    }

    public async Task<ProdutoServico?> ObterPorId(int id)
    {
        return await Context.ProdutoServicos
            .Include(c => c.Fornecedor)
            .Include(c => c.ProdutoServicoCaracteristicas)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<IResultadoPaginado<ProdutoServico>> Buscar(IBuscaPaginada<ProdutoServico> filtro)
    {
        var query = Context.ProdutoServicos
            .Include(c => c.Fornecedor)
            .Include(c => c.ProdutoServicoCaracteristicas)
            .AsQueryable();
        return await base.Buscar(query, filtro);
    }

    public void Remover(ProdutoServico produtoServico)
    {
        Context.ProdutoServicoCaracteristicas.RemoveRange(produtoServico.ProdutoServicoCaracteristicas);
        Context.ProdutoServicos.Remove(produtoServico);
    }
}