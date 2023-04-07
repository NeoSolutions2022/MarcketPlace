using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.ProdutoServico;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Fornecedor;

[Route("v{version:apiVersion}/Fornecedor/[controller]")]
public class ProdutosServicoController : MainController
{
    private readonly IProdutoServicoService _produtoServicoService;
    public ProdutosServicoController(INotificator notificator, IProdutoServicoService produtoServicoService) : base(notificator)
    {
        _produtoServicoService = produtoServicoService;
    }
    
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter Produto/Serviço - Fornecedor.", Tags = new [] { "Fornecedor - Produto-Serviço" })]
    [ProducesResponseType(typeof(ProdutoServicoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var produtoServico = await _produtoServicoService.ObterPorId(id);
        return OkResponse(produtoServico);
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Buscar Produto/Serviço - Fornecedor.", Tags = new [] { "Fornecedor - Produto-Serviço" })]
    [ProducesResponseType(typeof(PagedDto<ProdutoServicoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Buscar([FromQuery] BuscarProdutoServicoDto dto)
    {
        var produtoServico = await _produtoServicoService.Buscar(dto);
        return OkResponse(produtoServico);
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Cadastrar Produto/Serviço - Fornecedor.", Tags = new [] { "Fornecedor - Produto-Serviço" })]
    [ProducesResponseType(typeof(ProdutoServicoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Cadastrar([FromForm] CadastrarProdutoServicoDto dto)
    {
        var produtoServico = await _produtoServicoService.Adicionar(dto);
        return OkResponse(produtoServico);
    }
    
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Alterar Produto/Serviço - Fornecedor.", Tags = new [] { "Fornecedor - Produto-Serviço" })]
    [ProducesResponseType(typeof(ProdutoServicoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Alterar(int id, [FromBody] AlterarProdutoServicoDto dto)
    {
        var produtoServico = await _produtoServicoService.Alterar(id, dto);
        return OkResponse(produtoServico);
    }
    
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Deletar Produto/Serviço - Fornecedor.", Tags = new[] { "Fornecedor - Produto-Serviço" })]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Desativar(int id)
    {
        await _produtoServicoService.Desativar(id);
        return NoContentResponse();
    }
    
    [HttpPatch("{id}")]
    [SwaggerOperation(Summary = "Reativar Produto/Serviço - Fornecedor.", Tags = new[] { "Fornecedor - Produto-Serviço" })]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Reativar(int id)
    {
        await _produtoServicoService.Reativar(id);
        return NoContentResponse();
    }
}