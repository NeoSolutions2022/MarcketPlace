using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Base;
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
    
    [HttpGet]
    [SwaggerOperation(Summary = "Listagem de Clientes", Tags = new[] { "Gerencia - Cliente" })]
    [ProducesResponseType(typeof(PagedDto<ClienteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]

    public async Task<IActionResult> Buscar([FromQuery] BuscarClienteDto dto)
    {
        var cliente = await _clienteService.Buscar(dto);
        return OkResponse(cliente);
    }
    
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter um Cliente por Id.", Tags = new [] { "Gerencia - Cliente" })]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var usuario = await _clienteService.ObterPorId(id);
        return OkResponse(usuario);
    }
    
    [HttpGet("email/{email}")]
    [SwaggerOperation(Summary = "Obter um Cliente por Email.", Tags = new [] { "Gerencia - Cliente" })]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorEmail(string email)
    {
        var usuario = await _clienteService.ObterPorEmail(email);
        return OkResponse(usuario);
    }
    
    [HttpGet("cpf/{cpf}")]
    [SwaggerOperation(Summary = "Obter um Cliente por Cpf.", Tags = new [] { "Gerencia - Cliente" })]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorCpf(string cpf)
    {
        var usuario = await _clienteService.ObterPorCpf(cpf);
        return OkResponse(usuario);
    }
    
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar um Cliente.", Tags = new [] { "Gerencia - Cliente" })]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Alterar(int id, [FromForm] AlterarClienteDto dto)
    {
        var usuario = await _clienteService.Alterar(id, dto);
        return OkResponse(usuario);
    }
}