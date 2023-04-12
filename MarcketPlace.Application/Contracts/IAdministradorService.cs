using MarcketPlace.Application.Dtos.V1.Administrador;
using MarcketPlace.Application.Dtos.V1.Base;

namespace MarcketPlace.Application.Contracts;

public interface IAdministradorService
{
    Task<PagedDto<AdministradorDto>> Buscar(BuscarAdministradorDto dto);
    Task<AdministradorDto?> Adicionar(AdicionarAdministradorDto dto);
    Task<AdministradorDto?> Alterar(int id, AlterarAdministradorDto dto);
    Task<AdministradorDto?> ObterPorId(int id);
    Task<AdministradorDto?> ObterPorEmail(string email);
    Task Desaticar(int id);
    Task Reativar(int id);
    Task Remover(int id);
    Task AlterarSenha(int id);
}