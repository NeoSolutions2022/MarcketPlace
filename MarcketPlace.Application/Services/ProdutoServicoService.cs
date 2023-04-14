using System.Text;
using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.ProdutoServico;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Enums;
using MarcketPlace.Core.Extensions;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Services;

public class ProdutoServicoService : BaseService, IProdutoServicoService
{
    private readonly IProdutoServicoRepository _produtoServicoRepository;
    private readonly IFileService _fileService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProdutoServicoService(IMapper mapper, INotificator notificator,
        IProdutoServicoRepository produtoServicoRepository, IFileService fileService, IHttpContextAccessor httpContextAccessor) : base(mapper, notificator)
    {
        _produtoServicoRepository = produtoServicoRepository;
        _fileService = fileService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProdutoServicoDto?> Adicionar(CadastrarProdutoServicoDto dto)
    {
        var produtoServico = Mapper.Map<ProdutoServico>(dto);

        StringBuilder links = new StringBuilder();
        if (dto.Fotos.Count != 0)
        {
            foreach (var foto in dto.Fotos)
            {
                if (foto is { Length : > 0 })
                {
                    links.Append("&" + await _fileService.Upload(foto, EUploadPath.FotoProdutoServico));
                }
            }
        }

        produtoServico.FornecedorId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.ObterUsuarioId());
        produtoServico.Foto = links.ToString();
        produtoServico.CriadoEm = DateTime.Now;
        if (!await Validar(produtoServico))
        {
            return null;
        }
        
        _produtoServicoRepository.Adicionar(produtoServico);
        
        if (await _produtoServicoRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<ProdutoServicoDto>(produtoServico);
        }

        Notificator.Handle("Não foi possível salvar o produto ou serviço!");
        return null;
    }

    public async Task<ProdutoServicoDto?> Alterar(int id, AlterarProdutoServicoDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não conferem!");
            return null;
        }

        var produtoServico = await _produtoServicoRepository.ObterPorId(id);
        if (produtoServico == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        StringBuilder links = new StringBuilder();
        if (dto.Fotos.Count != 0)
        {
            foreach (var foto in dto.Fotos)
            {
                if (foto is { Length : > 0 })
                {
                    links.Append("*" + await _fileService.Upload(foto, EUploadPath.FotoProdutoServico));
                }
            }
        }

        produtoServico.Foto = links.ToString(); // TODO Identificar quais fotos foram alteradas
        Mapper.Map(produtoServico, dto);
        if (!await Validar(produtoServico))
        {
            return null;
        }

        produtoServico.AtualizadoEm = DateTime.Now;
        _produtoServicoRepository.Alterar(produtoServico);
        if (await _produtoServicoRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<ProdutoServicoDto>(produtoServico);
        }

        Notificator.Handle("Não foi possível alterar o produto ou serviço!");
        return null;
    }

    public async Task Desativar(int id)
    {
        var produtoServico = await _produtoServicoRepository.ObterPorId(id);
        if (produtoServico == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        produtoServico.Desativado = true;
        produtoServico.AtualizadoEm = DateTime.Now;
        _produtoServicoRepository.Alterar(produtoServico);
        if (await _produtoServicoRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível desativar o produto ou serviço");
    }

    public async Task Reativar(int id)
    {
        var produtoServico = await _produtoServicoRepository.ObterPorId(id);
        if (produtoServico == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        produtoServico.Desativado = false;
        produtoServico.AtualizadoEm = DateTime.Now;
        _produtoServicoRepository.Alterar(produtoServico);
        if (await _produtoServicoRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível reativar o produto ou serviço");
    }

    public async Task<PagedDto<ProdutoServicoDto>> Buscar(BuscarProdutoServicoDto dto)
    {
        var administrador = await _produtoServicoRepository.Buscar(dto);
        return Mapper.Map<PagedDto<ProdutoServicoDto>>(administrador);
    }

    public async Task<ProdutoServicoDto?> ObterPorId(int id)
    {
        var produtoServico = await _produtoServicoRepository.ObterPorId(id);
        if (produtoServico != null)
        {
            return Mapper.Map<ProdutoServicoDto>(produtoServico);
        }

        Notificator.HandleNotFoundResource();
        return null;
    }

    private async Task<bool> Validar(ProdutoServico produtoServico)
    {
        if (!produtoServico.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }

        if (produtoServico.FornecedorId != Convert.ToInt32(_httpContextAccessor.HttpContext?.User.ObterUsuarioId()))
        {
            Notificator.Handle("Você não tem permissão para executar essa ação!");
        }
        
        var administradorExistente =
            await _produtoServicoRepository.FistOrDefault(
                c => c.Titulo == produtoServico.Titulo && c.Id != produtoServico.Id);
        if (administradorExistente != null)
        {
            Notificator.Handle("Já existe um administrador com esse email!");
        }

        return !Notificator.HasNotification;
    }
}