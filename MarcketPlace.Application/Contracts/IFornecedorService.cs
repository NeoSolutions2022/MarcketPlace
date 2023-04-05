using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.Fornecedor;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Contracts;

public interface IFornecedorService
{
    Task<PagedDto<FornecedorDto>> Buscar(BuscarFornecedorDto dto);
    Task<FornecedorDto?> Cadastrar(CadastrarFornecedorDto dto);
    Task<FornecedorDto?> Alterar(int id, AlterarFornecedorDto dto);
    Task<FornecedorDto?> ObterPorId(int id);
    Task<FornecedorDto?> ObterProdutoServicos(int id);
    Task<FornecedorDto?> ObterPorEmail(string email);
    Task<FornecedorDto?> ObterPorCnpj(string cnpj);
    Task<FornecedorDto?> ObterPorCpf(string cpf);
    Task AlterarSenha(int id);
    Task Desativar(int id);
    Task AlterarDescricao(int id, string descricao);
    
    Task AlterarFoto(int id, IFormFile foto);
    Task Reativar(int id);
    Task Remover(int id);
}