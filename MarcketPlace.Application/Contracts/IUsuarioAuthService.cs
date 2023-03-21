using MarcketPlace.Application.Dtos.V1.Auth;

namespace MarcketPlace.Application.Contracts;

public interface IUsuarioAuthService
{
    Task<UsuarioAutenticadoDto?> LoginCliente(LoginDto loginDto);
    Task<UsuarioAutenticadoDto?> LoginFornecedor(LoginDto loginDto);
}