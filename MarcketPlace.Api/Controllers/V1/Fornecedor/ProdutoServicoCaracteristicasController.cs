using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.ProdutoServico.ProdutoServicoCaracteristica;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Authorization;
using MarcketPlace.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Fornecedor;

[Route("v{version:apiVersion}/Fornecedor/[controller]")]
public class ProdutoServicoCaracteristicasController : MainController
{
    private readonly IProdutoServicoCaracteristicaService _caracteristicaService;
    public ProdutoServicoCaracteristicasController(INotificator notificator, IProdutoServicoCaracteristicaService caracteristicaService) : base(notificator)
    {
        _caracteristicaService = caracteristicaService;
    }
    
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter Caracteristica Produto/Serviço - Fornecedor.", Tags = new[] { "Fornecedor - Produto-Serviço" })]
    [ProducesResponseType(typeof(ProdutoServicoCaracteristicaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var produtoServico = await _caracteristicaService.ObterPorId(id);
        return OkResponse(produtoServico);
    }

    [HttpGet("produtoServicoId/{produtoId}")]
    [SwaggerOperation(Summary = "Buscar Caracteristica Produto/Serviço - Fornecedor.",
        Tags = new[] { "Fornecedor - Produto-Serviço" })]
    [ProducesResponseType(typeof(List<ProdutoServicoCaracteristicaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObterTodos(int produtoId)
    {
        var produtoServico = await _caracteristicaService.ObterPorTodos(produtoId);
        return OkResponse(produtoServico);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cadastrar Caracteristica Produto/Serviço - Fornecedor.",
        Tags = new[] { "Fornecedor - Produto-Serviço" })]
    [ClaimsAuthorize("Fornecedor", ETipoUsuario.Fornecedor)]
    [ProducesResponseType(typeof(ProdutoServicoCaracteristicaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Cadastrar([FromForm] List<AdicionarProdutoServicoCaracteristicaDto> dto)
    {
        var produtoServico = await _caracteristicaService.Adicionar(dto);
        return OkResponse(produtoServico);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Alterar Caracteristica Produto/Serviço - Fornecedor.",
        Tags = new[] { "Fornecedor - Produto-Serviço" })]
    [ClaimsAuthorize("Fornecedor", ETipoUsuario.Fornecedor)]
    [ProducesResponseType(typeof(ProdutoServicoCaracteristicaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Alterar(int id, AlterarProdutoServicoCaracteristicaDto dto)
    {
        var produtoServico = await _caracteristicaService.Alterar(id, dto);
        return OkResponse(produtoServico);
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Remover uma Caracteristica.", Tags = new[] { "Fornecedor - Produto-Serviço" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Remover(int id)
    {
        await _caracteristicaService.Remover(id);
        return NoContentResponse();
    }
}