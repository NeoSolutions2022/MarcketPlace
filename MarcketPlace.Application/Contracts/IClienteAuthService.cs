using MarcketPlace.Application.Dtos.V1.Auth;
using MarcketPlace.Application.Dtos.V1.Cliente;

namespace MarcketPlace.Application.Contracts;

public interface IClienteAuthService
{
    Task<UsuarioAutenticadoDto?> Login(LoginDto loginDto);
    Task<bool> VerificarCodigo(VerificarCodigoResetarSenhaClienteDto dto);
    Task RecuperarSenha(RecuperarSenhaClienteDto dto);
    Task AlterarSenha(AlterarSenhaClienteDto dto);
}