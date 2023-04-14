using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Administrador;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.Fornecedor;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Gerencia;

public class AdministradoresController : MainController
{
    private readonly IAdministradorService _administradorService;

    public AdministradoresController(INotificator notificator, IAdministradorService administradorService) :
        base(notificator)
    {
        _administradorService = administradorService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Listagem de Administradores", Tags = new[] { "Gerencia - Administrador" })]
    [ProducesResponseType(typeof(PagedDto<FornecedorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Buscar([FromQuery] BuscarAdministradorDto dto)
    {
        var cliente = await _administradorService.Buscar(dto);
        return OkResponse(cliente);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter um Administrador por Id.", Tags = new[] { "Gerencia - Administrador" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var usuario = await _administradorService.ObterPorId(id);
        return OkResponse(usuario);
    }

    [HttpGet("email/{email}")]
    [SwaggerOperation(Summary = "Obter um Administrador por Email.", Tags = new[] { "Gerencia - Administrador" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorEmail(string email)
    {
        var usuario = await _administradorService.ObterPorEmail(email);
        return OkResponse(usuario);
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Remover um Administrador.", Tags = new[] { "Gerencia - Administrador" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Remover(int id)
    {
        await _administradorService.Remover(id);
        return NoContentResponse();
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Adicionar um Administrador.", Tags = new[] { "Gerencia - Administrador" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Adicionar(AdicionarAdministradorDto dto)
    {
        var administrador = await _administradorService.Adicionar(dto);
        return OkResponse(administrador);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Alterar um Administrador.", Tags = new[] { "Gerencia - Administrador" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Alterar(int id, AlterarAdministradorDto dto)
    {
        var administrador = await _administradorService.Alterar(id, dto);
        return OkResponse(administrador);
    }

    [HttpPatch("desativar-administrador")]
    [SwaggerOperation(Summary = "Desativar um Administrador.", Tags = new[] { "Gerencia - Administrador" })]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Desativar(int id)
    {
        await _administradorService.Desaticar(id);
        return NoContentResponse();
    }

    [HttpPatch("reativar-administrador")]
    [SwaggerOperation(Summary = "Reativar um Administrador.", Tags = new[] { "Gerencia - Administrador" })]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Reativar(int id)
    {
        await _administradorService.Reativar(id);
        return NoContentResponse();
    }

    [HttpPost("{id}/alterar-senha")]
    [SwaggerOperation(Summary = "Enviar email para alterar a senha.", Tags = new[] { "Gerencia - Administrador" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlterarSenha(int id)
    {
        await _administradorService.AlterarSenha(id);
        return OkResponse();
    }
}