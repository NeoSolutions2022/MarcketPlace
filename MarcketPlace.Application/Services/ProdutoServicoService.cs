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
        IProdutoServicoRepository produtoServicoRepository, IFileService fileService,
        IHttpContextAccessor httpContextAccessor) : base(mapper, notificator)
    {
        _produtoServicoRepository = produtoServicoRepository;
        _fileService = fileService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProdutoServicoDto?> Adicionar(CadastrarProdutoServicoDto dto)
    {
        var produtoServico = Mapper.Map<ProdutoServico>(dto);

        if (dto.Foto is { Length : > 0 })
        {
            produtoServico.Foto = await _fileService.Upload(dto.Foto, EUploadPath.FotoProdutoServico);
        }

        if (dto.Foto2 is { Length : > 0 })
        {
            produtoServico.Foto2 = await _fileService.Upload(dto.Foto2, EUploadPath.FotoProdutoServico);
        }

        if (dto.Foto3 is { Length : > 0 })
        {
            produtoServico.Foto3 = await _fileService.Upload(dto.Foto3, EUploadPath.FotoProdutoServico);
        }

        if (dto.Foto4 is { Length : > 0 })
        {
            produtoServico.Foto4 = await _fileService.Upload(dto.Foto4, EUploadPath.FotoProdutoServico);
        }

        if (dto.Foto5 is { Length : > 0 })
        {
            produtoServico.Foto5 = await _fileService.Upload(dto.Foto5, EUploadPath.FotoProdutoServico);
        }

        produtoServico.FornecedorId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.ObterUsuarioId());
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

        Mapper.Map(dto, produtoServico);

        if (dto.Foto is { Length : > 0 })
        {
            produtoServico.Foto = await _fileService.Upload(dto.Foto, EUploadPath.FotoProdutoServico);
        }
        else
        {
            produtoServico.Foto = null;
        }

        if (dto.Foto2 is { Length : > 0 })
        {
            produtoServico.Foto2 = await _fileService.Upload(dto.Foto2, EUploadPath.FotoProdutoServico);
        }
        else
        {
            produtoServico.Foto2 = null;
        }

        if (dto.Foto3 is { Length : > 0 })
        {
            produtoServico.Foto3 = await _fileService.Upload(dto.Foto3, EUploadPath.FotoProdutoServico);
        }
        else
        {
            produtoServico.Foto3 = null;
        }

        if (dto.Foto4 is { Length : > 0 })
        {
            produtoServico.Foto4 = await _fileService.Upload(dto.Foto4, EUploadPath.FotoProdutoServico);
        }
        else
        {
            produtoServico.Foto4 = null;
        }

        if (dto.Foto5 is { Length : > 0 })
        {
            produtoServico.Foto5 = await _fileService.Upload(dto.Foto5, EUploadPath.FotoProdutoServico);
        }
        else
        {
            produtoServico.Foto5 = null;
        }

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

    public async Task AlterarFoto(AlterarFotoProdutoServicoDto dto)
    {
        var produtoServico = await _produtoServicoRepository.ObterPorId(dto.Id);
        if (produtoServico is null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        if (dto.Foto is { Length : > 0 })
        {
            produtoServico.Foto = await _fileService.Upload(dto.Foto, EUploadPath.FotoProdutoServico);
        }

        if (dto.Foto2 is { Length : > 0 })
        {
            produtoServico.Foto2 = await _fileService.Upload(dto.Foto2, EUploadPath.FotoProdutoServico);
        }

        if (dto.Foto3 is { Length : > 0 })
        {
            produtoServico.Foto3 = await _fileService.Upload(dto.Foto3, EUploadPath.FotoProdutoServico);
        }

        if (dto.Foto4 is { Length : > 0 })
        {
            produtoServico.Foto4 = await _fileService.Upload(dto.Foto4, EUploadPath.FotoProdutoServico);
        }

        if (dto.Foto5 is { Length : > 0 })
        {
            produtoServico.Foto5 = await _fileService.Upload(dto.Foto5, EUploadPath.FotoProdutoServico);
        }

        _produtoServicoRepository.Alterar(produtoServico);
        if (await _produtoServicoRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível alterar as fotos");
    }

    public async Task RemoverFoto(RemoverFotosProdutoServicoDto dto)
    {
        var produtoServico = await _produtoServicoRepository.ObterPorId(dto.Id);
        if (produtoServico is null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        switch (dto.IndexFoto)
        {
            case 1:
                produtoServico.Foto = null;
                break;
            case 2:
                produtoServico.Foto2 = null;
                break;
            case 3:
                produtoServico.Foto3 = null;
                break;
            case 4:
                produtoServico.Foto4 = null;
                break;
            case 5:
                produtoServico.Foto5 = null;
                break;
            default:
            {
                Notificator.Handle("Não foi possível alterar a foto");
                return;
            }
        }

        _produtoServicoRepository.Alterar(produtoServico);

        if (await _produtoServicoRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível alterar as fotos");
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

    public async Task Remover(int id)
    {
        var produtoServico = await _produtoServicoRepository.ObterPorId(id);
        if (produtoServico is null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }
        
        
        _produtoServicoRepository.Remover(produtoServico);
        if (await _produtoServicoRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível remover o produto ou serviço");
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