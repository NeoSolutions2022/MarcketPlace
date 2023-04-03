using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.Cliente;

namespace MarcketPlace.Application.Contracts;

public interface IClienteService
{
    Task<PagedDto<ClienteDto>> Buscar(BuscarClienteDto dto);
    Task<ClienteDto?> Cadastrar(CadastrarClienteDto dto);
    Task<ClienteDto?> Alterar(int id, AlterarClienteDto dto);
    Task<ClienteDto?> ObterPorId(int id);
    Task<ClienteDto?> ObterPorEmail(string email);
    Task<ClienteDto?> ObterPorCpf(string cpf);
    public Task AlterarSenha(int id);
    Task Desativar(int id);
    Task Reativar(int id);
    Task Remover(int id);
}