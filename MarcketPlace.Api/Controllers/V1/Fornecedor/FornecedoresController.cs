using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.Fornecedor;
using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarcketPlace.Api.Controllers.V1.Fornecedor;

[Route("v{version:apiVersion}/Fornecedor/[controller]")]
public class FornecedoresController : MainController
{
    private readonly IFornecedorService _fornecedorService;
    
    public FornecedoresController(INotificator notificator, IFornecedorService fornecedorService) : base(notificator)
    {
        _fornecedorService = fornecedorService;
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Listagem de Fornecedor", Tags = new[] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(typeof(PagedDto<FornecedorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]

    public async Task<IActionResult> Buscar([FromQuery] BuscarFornecedorDto dto)
    {
        var fornecedor = await _fornecedorService.Buscar(dto);
        return OkResponse(fornecedor);
    }
    
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter um Fornecedor por Id.", Tags = new [] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var fornecedor = await _fornecedorService.ObterPorId(id);
        return OkResponse(fornecedor);
    }
    
    [HttpGet("email/{email}")]
    [SwaggerOperation(Summary = "Obter um Fornecedor por Email.", Tags = new [] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorEmail(string email)
    {
        var fornecedor = await _fornecedorService.ObterPorEmail(email);
        return OkResponse(fornecedor);
    }
    
    [HttpGet("cnpj/{cnpj}")]
    [SwaggerOperation(Summary = "Obter um Fornecedor por Cnpj.", Tags = new [] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorCnpj(string cnpj)
    {
        var fornecedor = await _fornecedorService.ObterPorCnpj(cnpj);
        return OkResponse(fornecedor);
    }
    
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualizar um Fornecedor.", Tags = new [] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Alterar(int id, [FromForm] AlterarFornecedorDto dto)
    {
        var fornecedor = await _fornecedorService.Alterar(id, dto);
        return OkResponse(fornecedor);
    }
    
    [HttpPost("{id}/resetar-senha")]
    [SwaggerOperation(Summary = "Resetar a senha do Fornecedor.", Tags = new [] { "Gerencia - Fornecedor" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetarSenha(int id)
    {
        await _fornecedorService.ResetarSenha(id);
        return OkResponse();
    }
}