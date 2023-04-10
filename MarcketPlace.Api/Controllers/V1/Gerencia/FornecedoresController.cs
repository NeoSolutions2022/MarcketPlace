using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Fornecedor;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Gerencia;

[Route("v{version:apiVersion}/Gerencia/[controller]")]
public class FornecedoresController : MainController
{
    private readonly IFornecedorService _fornecedorService;

    public FornecedoresController(INotificator notificator, IFornecedorService fornecedorService) : base(notificator)
    {
        _fornecedorService = fornecedorService;
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter um Fornecedor por Id.", Tags = new[] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var fornecedor = await _fornecedorService.ObterProdutoServicos(id);
        return OkResponse(fornecedor);
    }
    
    [HttpPatch("ativar/{id}")]
    [SwaggerOperation(Summary = "Desativar um Fornecedor.", Tags = new[] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Desativar(int id)
    {
        await _fornecedorService.Desativar(id);
        return NoContentResponse();
    }

    [HttpPatch("reativar/{id}")]
    [SwaggerOperation(Summary = "Reativar um Fornecedor.", Tags = new[] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Reativar(int id)
    {
        await _fornecedorService.Reativar(id);
        return NoContentResponse();
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Remover um Fornecedor.", Tags = new[] { "Gerencia - Cliente" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Remover(int id)
    {
        await _fornecedorService.Remover(id);
        return NoContentResponse();
    }
}