using MarcketPlace.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Core.Extensions;

public static class HttpContextAccessorExtensions
{
    public static bool UsuarioAutenticado(this IHttpContextAccessor? contextAccessor)
    {
        return contextAccessor?.HttpContext?.User?.UsuarioAutenticado() ?? false;
    }

    public static int? ObterUsuarioId(this IHttpContextAccessor? contextAccessor)
    {
        var id = contextAccessor?.HttpContext?.User?.ObterUsuarioId() ?? string.Empty;
        return string.IsNullOrWhiteSpace(id) ? null : int.Parse(id);
    }

    public static ETipoUsuario? ObterTipoUsuario(this IHttpContextAccessor? contextAccessor)
    {
        var tipo = contextAccessor?.HttpContext?.User?.ObterTipoUsuario() ?? string.Empty;
        return string.IsNullOrWhiteSpace(tipo) ? null : Enum.Parse<ETipoUsuario>(tipo);
    }

    public static ETipoUsuario? ObterTipoAdministrador(this IHttpContextAccessor? contextAccessor)
    {
        var tipo = contextAccessor?.HttpContext?.User?.ObterTipoAdministrador() ?? string.Empty;
        return string.IsNullOrWhiteSpace(tipo) ? null : Enum.Parse<ETipoUsuario>(tipo);
    }

    public static ETipoUsuario? ObterTipoFornecedor(this IHttpContextAccessor? contextAccessor)
    {
        var tipo = contextAccessor?.HttpContext?.User?.ObterTipoFornecedor() ?? string.Empty;
        return string.IsNullOrWhiteSpace(tipo) ? null : Enum.Parse<ETipoUsuario>(tipo);
    }

    public static ETipoUsuario? ObterTipoCliente(this IHttpContextAccessor? contextAccessor)
    {
        var tipo = contextAccessor?.HttpContext?.User?.ObterTipoCliente() ?? string.Empty;
        return string.IsNullOrWhiteSpace(tipo) ? null : Enum.Parse<ETipoUsuario>(tipo);
    }

    public static bool EhAdministrador(this IHttpContextAccessor? contextAccessor)
    {
        return ObterTipoUsuario(contextAccessor) is ETipoUsuario.Administrador;
    }
}