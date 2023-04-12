using MarcketPlace.Domain.Contracts.Paginacao;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using MarcketPlace.Infra.Abstractions;
using MarcketPlace.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MarcketPlace.Infra.Repositories;

public class AdministradorRepository : Repository<Administrador>, IAdministradorRepository
{
    public AdministradorRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public void Adicionar(Administrador administrador)
    {
        Context.Administradores.Add(administrador);
    }

    public void Alterar(Administrador administrador)
    {
        Context.Administradores.Update(administrador);
    }

    public void Remover(Administrador administrador)
    {
        Context.Administradores.Remove(administrador);
    }

    public async Task<Administrador?> ObterPorId(int id)
    {
        return await Context.Administradores.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Administrador?> ObterPorEmail(string email)
    {
        return await Context.Administradores.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IResultadoPaginado<Administrador>> Buscar(IBuscaPaginada<Administrador> filtro)
    {
        var query = Context.Administradores.AsQueryable();
        return await base.Buscar(query, filtro);
    }
}