using System.Runtime.Serialization.Formatters;
using System.Text;
using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.ProdutoServico;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Enums;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using Microsoft.Extensions.Azure;

namespace MarcketPlace.Application.Services;

public class ProdutoServicoService : BaseService, IProdutoServicoService
{
    private readonly IProdutoServicoRepository _produtoServicoRepository;
    private readonly IFileService _fileService;

    public ProdutoServicoService(IMapper mapper, INotificator notificator,
        IProdutoServicoRepository produtoServicoRepository, IFileService fileService) : base(mapper, notificator)
    {
        _produtoServicoRepository = produtoServicoRepository;
        _fileService = fileService;
    }

    public async Task<ProdutoServicoDto?> Adicionar(CadastrarProdutoServicoDto dto)
    {
        var produtoServico = Mapper.Map<ProdutoServico>(dto);
        if (!await Validar(produtoServico))
        {
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

        produtoServico.Foto = links.ToString();
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