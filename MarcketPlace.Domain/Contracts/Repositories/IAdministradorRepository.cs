using MarcketPlace.Domain.Contracts.Paginacao;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Contracts.Repositories;

public interface IAdministradorRepository : IRepository<Administrador>
{
    void Adicionar(Administrador administrador);
    void Alterar(Administrador administrador);
    void Remover(Administrador administrador);
    Task<Administrador?> ObterPorId(int id);
    Task<Administrador?> ObterPorEmail(string email);
    Task<IResultadoPaginado<Administrador>> Buscar(IBuscaPaginada<Administrador> filtro);
}