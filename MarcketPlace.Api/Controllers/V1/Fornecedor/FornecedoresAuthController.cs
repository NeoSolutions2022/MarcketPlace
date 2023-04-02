using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Auth;
using MarcketPlace.Application.Dtos.V1.Fornecedor;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Fornecedor;

[AllowAnonymous]
[Route("v{version:apiVersion}/Fornecedor/[controller]")]
public class FornecedorAuthController : BaseController
{
    private readonly IFornecedorAuthService _fornecedorAuthService;

    public FornecedorAuthController(INotificator notificator, IFornecedorAuthService fornecedorAuthService) :
        base(notificator)
    {
        _fornecedorAuthService = fornecedorAuthService;
    }

    [HttpPost("Login-Fornecedor")]
    [SwaggerOperation(Summary = "Login - Fornecedor.", Tags = new[] { "Usuario - Fornecedor - Autenticação" })]
    [ProducesResponseType(typeof(UsuarioAutenticadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginFornecedor([FromBody] LoginDto loginFornecedor)
    {
        var token = await _fornecedorAuthService.Login(loginFornecedor);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Usuário e/ou senha incorretos" });
    }

    [HttpPost("verificar-codigo")]
    [SwaggerOperation(Summary = "Verifica o código para resetar a senha.",
        Tags = new[] { "Usuario - Fornecedor - Autenticação" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerificarCodigo([FromBody] VerificarCodigoResetarSenhaFornecedorDto dto)
    {
        await _fornecedorAuthService.VerificarCodigo(dto);
        return OkResponse();
    }

    [HttpPost("recuperar-senha")]
    [SwaggerOperation(Summary = "Enviar email para recuperar a senha.",
        Tags = new[] { "Usuario - Fornecedor - Autenticação" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RecuperarSenha([FromBody] RecuperarSenhaFornecedorDto dto)
    {
        await _fornecedorAuthService.RecuperarSenha(dto);
        return OkResponse();
    }

    [HttpPost("alterar-senha")]
    [SwaggerOperation(Summary = "Alterar a senha do usuario.", Tags = new[] { "Usuario - Fornecedor - Autenticação" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaFornecedorDto dto)
    {
        await _fornecedorAuthService.AlterarSenha(dto);
        return OkResponse();
    }
}