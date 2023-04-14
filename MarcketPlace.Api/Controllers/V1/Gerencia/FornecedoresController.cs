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
    
    [HttpPatch("desativar/{id}")]
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
    
    [HttpPatch("ativar-anuncio/{id}")]
    [SwaggerOperation(Summary = "Ativar anúncio para um Fornecedor.", Tags = new[] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AtivarAnuncio(int id)
    {
        await _fornecedorService.AtivarAnuncio(id);
        return NoContentResponse();
    }
    
    [HttpPatch("desativar-anuncio/{id}")]
    [SwaggerOperation(Summary = "Desativar anúncio para um Fornecedor.", Tags = new[] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DesativarAnuncio(int id)
    {
        await _fornecedorService.DesativarAnuncio(id);
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