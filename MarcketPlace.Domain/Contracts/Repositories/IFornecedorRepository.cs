using MarcketPlace.Domain.Contracts.Paginacao;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Contracts.Repositories;

public interface IFornecedorRepository : IRepository<Fornecedor>
{
    void Adicionar(Fornecedor fornecedor);
    void Alterar(Fornecedor fornecedor);
    Task<Fornecedor?> ObterPorId(int id);
    Task<Fornecedor?> ObterPorEmail(string email);
    Task<Fornecedor?> ObterPorCpf(string cpf);
    Task<Fornecedor?> ObterPorCnpj(string cnpj);
    void Remover(Fornecedor fornecedor);
    Task<IResultadoPaginado<Fornecedor>> Buscar(IBuscaPaginada<Fornecedor> filtro);
}