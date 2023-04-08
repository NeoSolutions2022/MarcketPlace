using System.ComponentModel;
using System.Security.Claims;
using MarcketPlace.Core.Enums;
using MarcketPlace.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MarcketPlace.Core.Authorization;

public static class CustomAuthorization
{
    public static bool ValidateUserClaims(HttpContext context, string claimName, string claimValue)
    {
        return context.User.Identity!.IsAuthenticated &&
               context.User.Permissoes().Any(c => c.Nome == claimName && c.Tipo.Contains(claimValue));
    }

    public static bool ValidateUserType(HttpContext context, string claimName, string claimValue)
    {
        return claimName switch
        {
            "Administrador" => context.User.Identity!.IsAuthenticated &&
                               context.User.ObterTipoAdministrador() == claimValue,
            "Fornecedor" => context.User.Identity!.IsAuthenticated && context.User.ObterTipoFornecedor() == claimValue,
            _ => context.User.Identity!.IsAuthenticated && context.User.ObterTipoCliente() == claimValue
        };
    }
}

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequirementClaimFilter))
    {
        Arguments = new object[] { new Claim(claimName, claimValue) };
    }

    public ClaimsAuthorizeAttribute(string claimName, EPermissaoTipo value) : base(typeof(RequirementClaimFilter))
    {
        Arguments = new object[] { new Claim(claimName, value.ToDescriptionString()) };
    }

    public ClaimsAuthorizeAttribute(string claimName, ETipoUsuario claimValue) : base(typeof(RequirementClaimFilter))
    {
        Arguments = new object[] { new Claim(claimName, claimValue.ToDescriptionString()) };
    }
}

public class RequirementClaimFilter : IAuthorizationFilter
{
    private readonly Claim _claim;

    public RequirementClaimFilter(Claim claim)
    {
        _claim = claim;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (_claim.Type == "Administrador")
        {
            if (!CustomAuthorization.ValidateUserType(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }

            return;
        }

        if (_claim.Type == "Fornecedor")
        {
            if (!CustomAuthorization.ValidateUserType(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }

            return;
        }

        if (_claim.Type == "Cliente")
        {
            if (!CustomAuthorization.ValidateUserType(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }

            return;
        }

        if (!context.HttpContext.User.Identity!.IsAuthenticated)
        {
            context.Result = new StatusCodeResult(401);
            return;
        }

        if (!CustomAuthorization.ValidateUserClaims(context.HttpContext, _claim.Type, _claim.Value))
        {
            context.Result = new StatusCodeResult(403);
        }
    }
}

public class PermissaoClaim
{
    public string Nome { get; set; } = null!;
    public string Tipo { get; set; } = null!;
}

public enum EPermissaoTipo
{
    [Description("R")] Read = 1,

    [Description("W")] Write = 2,

    [Description("D")] Delete = 3,

    [Description("RW")] ReadWrite = 4,

    [Description("WD")] WriteDelete = 5,

    [Description("RD")] ReadDelete = 6,

    [Description("RWD")] Full = 7
}

public enum EPemissaoNivel
{
    R = 1,
    W = 2,
    D = 3
}