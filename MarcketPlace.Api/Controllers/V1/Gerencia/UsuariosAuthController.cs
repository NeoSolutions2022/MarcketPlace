using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Auth;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Gerencia;

[AllowAnonymous]
[Route("v{version:apiVersion}/Gerencia/[controller]")]
public class UsuariosAuthController : BaseController
{
    private readonly IUsuarioAuthService _usuarioAuthService;
    
    public UsuariosAuthController(INotificator notificator, IUsuarioAuthService usuarioAuthService) : base(notificator)
    {
        _usuarioAuthService = usuarioAuthService;
    }
    
    [HttpPost("Login-Cliente")]
    [SwaggerOperation(Summary = "Login - Cliente.", Tags = new [] { "Gerencia - Cliente Autenticação" })]
    [ProducesResponseType(typeof(UsuarioAutenticadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginCliente([FromBody] LoginDto loginCliente)
    {
        var token = await _usuarioAuthService.LoginCliente(loginCliente);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Usuário e/ou senha incorretos" });
    }
    
    [HttpPost("Login-Fornecedor")]
    [SwaggerOperation(Summary = "Login - Fornecedor.", Tags = new [] { "Gerencia - Fornecedor Autenticação" })]
    [ProducesResponseType(typeof(UsuarioAutenticadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginFornecedor([FromBody] LoginDto loginFornecedor)
    {
        var token = await _usuarioAuthService.LoginFornecedor(loginFornecedor);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Usuário e/ou senha incorretos" });
    }
}