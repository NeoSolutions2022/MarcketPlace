using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Auth;
using MarcketPlace.Application.Dtos.V1.Cliente;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Cliente;

[AllowAnonymous]
[Route("v{version:apiVersion}/Cliente/[controller]")]
public class ClientesAuthController : BaseController
{
    private readonly IClienteAuthService _clienteAuthService;

    public ClientesAuthController(INotificator notificator, IClienteAuthService clienteAuthService) : base(notificator)
    {
        _clienteAuthService = clienteAuthService;
    }

    [HttpPost("Login-Cliente")]
    [SwaggerOperation(Summary = "Login - Cliente.", Tags = new[] { "Usuario - Cliente - Autenticação" })]
    [ProducesResponseType(typeof(UsuarioAutenticadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginCliente([FromBody] LoginDto loginCliente)
    {
        var token = await _clienteAuthService.Login(loginCliente);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Usuário e/ou senha incorretos" });
    }

    [HttpPost("verificar-codigo")]
    [SwaggerOperation(Summary = "Verifica o código para resetar a senha.",
        Tags = new[] { "Usuario - Cliente - Autenticação" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerificarCodigo([FromBody] VerificarCodigoResetarSenhaClienteDto dto)
    {
        await _clienteAuthService.VerificarCodigo(dto);
        return OkResponse();
    }

    [HttpPost("recuperar-senha")]
    [SwaggerOperation(Summary = "Enviar email para recuperar a senha.",
        Tags = new[] { "Usuario - Cliente - Autenticação" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RecuperarSenha([FromBody] RecuperarSenhaClienteDto dto)
    {
        await _clienteAuthService.RecuperarSenha(dto);
        return OkResponse();
    }

    [HttpPost("alterar-senha")]
    [SwaggerOperation(Summary = "Alterar a senha do usuario.", Tags = new[] { "Usuario - Cliente - Autenticação" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaClienteDto dto)
    {
        await _clienteAuthService.AlterarSenha(dto);
        return OkResponse();
    }
}