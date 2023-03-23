using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Gerencia;

[Route("v{version:apiVersion}/Gerencia/[controller]")]
public class ClientesController : MainController
{
    private readonly IClienteService _clienteService;
    
    public ClientesController(INotificator notificator, IClienteService clienteService) : base(notificator)
    {
        _clienteService = clienteService;
    }

    [HttpPatch("ativar/{id}")]
    [SwaggerOperation(Summary = "Desativar um Cliente.", Tags = new [] { "Gerencia - Cliente" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Desativar(int id)
    {
        await _clienteService.Desativar(id);
        return NoContentResponse();
    }
    
    [HttpPatch("reativar/{id}")]
    [SwaggerOperation(Summary = "Reativar um Cliente.", Tags = new [] { "Gerencia - Cliente" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Reativar(int id)
    {
        await _clienteService.Reativar(id);
        return NoContentResponse();
    }
}