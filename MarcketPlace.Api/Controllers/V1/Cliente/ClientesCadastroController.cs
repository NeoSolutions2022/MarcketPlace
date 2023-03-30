using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Cliente;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Cliente;

[Route("v{version:apiVersion}/Cliente/[controller]")]
public class ClientesCadastroController : BaseController
{
    private readonly IClienteService _clienteService;
    
    public ClientesCadastroController(INotificator notificator, IClienteService clienteService) : base(notificator)
    {
        _clienteService = clienteService;
    }
    
    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Cadastro de um Cliente.", Tags = new [] { "Usuario - Cliente" })]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Cadastrar([FromForm] CadastrarClienteDto dto)
    {
        var usuario = await _clienteService.Cadastrar(dto);
        return CreatedResponse("", usuario);
    }
}