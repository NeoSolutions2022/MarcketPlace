using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Cliente;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Cliente;

[Route("v{version:apiVersion}/Cliente/[controller]")]
public class ClientesController : MainController
{
    private readonly IClienteService _clienteService;

    public ClientesController(INotificator notificator, IClienteService clienteService) : base(notificator)
    {
        _clienteService = clienteService;
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar um Cliente.", Tags = new[] { "Usuario - Cliente" })]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Alterar(int id, [FromForm] AlterarClienteDto dto)
    {
        var usuario = await _clienteService.Alterar(id, dto);
        return OkResponse(usuario);
    }

    [HttpPost("{id}/alterar-senha")]
    [SwaggerOperation(Summary = "Enviar email para alterar a senha.", Tags = new[] { "Usuario - Cliente" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlterarSenha(int id)
    {
        await _clienteService.AlterarSenha(id);
        return OkResponse();
    }
}