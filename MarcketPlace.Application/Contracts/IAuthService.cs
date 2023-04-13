using MarcketPlace.Application.Dtos.V1.Administrador;
using MarcketPlace.Application.Dtos.V1.Auth;

namespace MarcketPlace.Application.Contracts;

public interface IAuthService
{
    Task<AdministradorAutenticadoDto?> LoginAdministrador(LoginDto loginDto);
    Task<bool> VerificarCodigo(VerificarCodigoResetarSenhaAdministradorDto administradorDto);
    Task RecuperarSenha(RecuperarSenhaAdministradorDto dto);
    Task AlterarSenha(AlterarSenhaAdministradorDto dto);
}