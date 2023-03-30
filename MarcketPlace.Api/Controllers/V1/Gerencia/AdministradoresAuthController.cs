using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Auth;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Gerencia;

[AllowAnonymous]
[Route("v{version:apiVersion}/Gerencia/[controller]")]
public class AdministradoresAuthController : BaseController
{
    private readonly IAuthService _authService;
    
    public AdministradoresAuthController(INotificator notificator, IAuthService authService) : base(notificator)
    {
        _authService = authService;
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Login - Administrador.", Tags = new [] { "Gerencia - Administrador Autenticação" })]
    [ProducesResponseType(typeof(AdministradorAutenticadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginAdministrador)
    {
        var token = await _authService.LoginAdministrador(loginAdministrador);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Usuário e/ou senha incorretos" });
    }
}