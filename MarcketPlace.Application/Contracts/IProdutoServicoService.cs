using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.ProdutoServico;
using MarcketPlace.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Contracts;

public interface IProdutoServicoService
{
    Task<ProdutoServicoDto?> Adicionar(CadastrarProdutoServicoDto dto);
    Task<ProdutoServicoDto?> Alterar(int id, AlterarProdutoServicoDto dto);
    Task<ProdutoServicoDto?> ObterPorId(int id);
    Task Desativar(int id);
    Task Reativar(int id);
    Task Remover(int id);
    Task<PagedDto<ProdutoServicoDto>> Buscar(BuscarProdutoServicoDto dto);
    Task AlterarFoto(AlterarFotoProdutoServicoDto dto);
    Task RemoverFoto(RemoverFotosProdutoServicoDto dto);
}