using MarcketPlace.Application.Dtos.V1.Auth;
using MarcketPlace.Application.Dtos.V1.Fornecedor;

namespace MarcketPlace.Application.Contracts;

public interface IFornecedorAuthService
{
    Task<UsuarioAutenticadoDto?> Login(LoginDto loginDto);
    Task<bool> VerificarCodigo(VerificarCodigoResetarSenhaFornecedorDto dto);
    Task RecuperarSenha(RecuperarSenhaFornecedorDto dto);
    Task AlterarSenha(AlterarSenhaFornecedorDto dto);
}