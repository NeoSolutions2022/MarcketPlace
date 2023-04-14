using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.Fornecedor;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Authorization;
using MarcketPlace.Core.Enums;
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
    [SwaggerOperation(Summary = "Listagem de Fornecedor", Tags = new[] { "Usuario - Fornecedor" })]
    [ProducesResponseType(typeof(PagedDto<FornecedorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Buscar([FromQuery] BuscarFornecedorDto dto)
    {
        var fornecedor = await _fornecedorService.Buscar(dto);
        return OkResponse(fornecedor);
    }
    
    [HttpGet("anuncios-fornecedores")]
    [SwaggerOperation(Summary = "Listagem dos anúncios de fornecedores", Tags = new[] { "Usuario - Fornecedor" })]
    [ProducesResponseType(typeof(PagedDto<FornecedorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> BuscarAnuncio()
    {
        var fornecedor = await _fornecedorService.BuscarAnuncio();
        return OkResponse(fornecedor);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obter um Fornecedor por Id.", Tags = new[] { "Usuario - Fornecedor" })]
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
    [SwaggerOperation(Summary = "Obter um Fornecedor por Email.", Tags = new[] { "Usuario - Fornecedor" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorEmail(string email)
    {
        var fornecedor = await _fornecedorService.ObterPorEmail(email);
        return OkResponse(fornecedor);
    }

    [HttpGet("cpf/{cpf}")]
    [SwaggerOperation(Summary = "Obter um Fornecedor por cpf.", Tags = new[] { "Usuario - Fornecedor" })]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorCpf(string cpf)
    {
        var fornecedor = await _fornecedorService.ObterPorCpf(cpf);
        return OkResponse(fornecedor);
    }

    [HttpGet("cnpj/{cnpj}")]
    [SwaggerOperation(Summary = "Obter um Fornecedor por Cnpj.", Tags = new[] { "Usuario - Fornecedor" })]
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
    [SwaggerOperation(Summary = "Atualizar um Fornecedor.", Tags = new[] { "Usuario - Fornecedor" })]
    [ClaimsAuthorize("Fornecedor", ETipoUsuario.Fornecedor)]
    [ProducesResponseType(typeof(FornecedorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Alterar(int id, [FromForm] AlterarFornecedorDto dto)
    {
        var fornecedor = await _fornecedorService.Alterar(id, dto);
        return OkResponse(fornecedor);
    }

    [HttpPatch("{id}/alterar-descricao")]
    [SwaggerOperation(Summary = "Alterar descrição.", Tags = new[] { "Usuario - Fornecedor" })]
    [ClaimsAuthorize("Fornecedor", ETipoUsuario.Fornecedor)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AlterarDescricao(int id, [FromBody] string descricao)
    {
        await _fornecedorService.AlterarDescricao(id, descricao);
        return OkResponse();
    }

    [HttpPatch("{id}/alterar-foto")]
    [SwaggerOperation(Summary = "Alterar foto.", Tags = new[] { "Usuario - Fornecedor" })]
    [ClaimsAuthorize("Fornecedor", ETipoUsuario.Fornecedor)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AlterarFoto(int id, [FromForm] AlterarFotoFornecedorDto foto)
    {
        await _fornecedorService.AlterarFoto(id, foto);
        return OkResponse();
    }

    [HttpPost("{id}/alterar-senha")]
    [SwaggerOperation(Summary = "Enviar email para alterar a senha.", Tags = new[] { "Usuario - Fornecedor" })]
    [ClaimsAuthorize("Fornecedor", ETipoUsuario.Fornecedor)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlterarSenha(int id)
    {
        await _fornecedorService.AlterarSenha(id);
        return OkResponse();
    }
}