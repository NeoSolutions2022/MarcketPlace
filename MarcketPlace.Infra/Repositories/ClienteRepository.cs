using MarcketPlace.Domain.Contracts.Paginacao;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using MarcketPlace.Infra.Abstractions;
using MarcketPlace.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MarcketPlace.Infra.Repositories;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    public ClienteRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public void Adicionar(Cliente cliente)
    {
        Context.Clientes.Add(cliente);
    }

    public void Alterar(Cliente cliente)
    {
        Context.Clientes.Update(cliente);
    }

    public async Task<Cliente?> ObterPorId(int id)
    {
        return await Context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cliente?> ObterPorEmail(string email)
    {
        return await Context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Cliente?> ObterPorCpf(string cpf)
    {
        return await Context.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);
    }

    public void Remover(Cliente cliente)
    {
        Context.Clientes.Remove(cliente);
    }

    public async Task<IResultadoPaginado<Cliente>> Buscar(IBuscaPaginada<Cliente> filtro)
    {
        var query = Context.Clientes.AsQueryable();
        return await base.Buscar(query, filtro);
    }
}