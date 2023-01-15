using MarcketPlace.Core.Enums;
using MarcketPlace.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Core.Authorization;

public interface IAuthenticatedUser
{
    public int Id { get; }
    public ETipoUsuario? TipoUsuario { get; }
    public bool UsuarioLogado { get; }
    public bool UsuarioComum { get; }
    public bool UsuarioAdministrador { get; }
}

public class AuthenticatedUser : IAuthenticatedUser
{
    public int Id { get; } = -1;
    public ETipoUsuario? TipoUsuario { get; }
    public bool UsuarioLogado => Id > 0;
    public bool UsuarioComum => TipoUsuario is ETipoUsuario.Comum;
    public bool UsuarioAdministrador => TipoUsuario is ETipoUsuario.Administrador;

    public AuthenticatedUser()
    { }

    public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
    {
        Id = httpContextAccessor.ObterUsuarioId()!.Value;
        TipoUsuario = httpContextAccessor.ObterTipoUsuario()!.Value;
    }
}