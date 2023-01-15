using MarcketPlace.Application.Dtos.V1.Auth;

namespace MarcketPlace.Application.Contracts;

public interface IAuthService
{
    Task<AdministradorAutenticadoDto?> LoginAdministrador(LoginDto loginDto);
    //Task<ClienteAutenticadoDto?> Login(LoginDto loginDto);
    //Task<FornecedortAutenticadoDto?> Login(LoginDto loginDto);
}