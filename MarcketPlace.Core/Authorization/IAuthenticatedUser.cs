using MarcketPlace.Core.Enums;
using MarcketPlace.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Core.Authorization;

public interface IAuthenticatedUser
{
    public int Id { get; }
    public ETipoUsuario? Administrador { get; }
    public ETipoUsuario? Fornecedor { get; }
    public ETipoUsuario? Cliente { get; }
    public bool UsuarioLogado { get; }
    public bool UsuarioAdministrador { get; }
    public bool UsuarioFornecedor { get; }
    public bool UsuarioCliente { get; }
}

public class AuthenticatedUser : IAuthenticatedUser
{
    public int Id { get; } = -1;
    public ETipoUsuario? Administrador { get; }
    public ETipoUsuario? Fornecedor { get; }
    public ETipoUsuario? Cliente { get; }
    public ETipoUsuario? TipoUsuario { get; }
    public bool UsuarioLogado => Id > 0;
    public bool UsuarioCliente => TipoUsuario is ETipoUsuario.Cliente;
    public bool UsuarioFornecedor => TipoUsuario is ETipoUsuario.Fornecedor;
    public bool UsuarioAdministrador => TipoUsuario is ETipoUsuario.Administrador;

    public AuthenticatedUser()
    {
    }

    public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
    {
        Id = httpContextAccessor.ObterUsuarioId()!.Value;
        TipoUsuario = httpContextAccessor.ObterTipoUsuario()!.Value;
        Administrador = httpContextAccessor.ObterTipoAdministrador()!.Value;
        Fornecedor = httpContextAccessor.ObterTipoFornecedor()!.Value;
        Cliente = httpContextAccessor.ObterTipoCliente()!.Value;
    }
}