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

        if (dto.Fotos.Count > 5)
        {
            Notificator.Handle("O número máximo de fotos permitidas é 5");
            return null;
        }
        
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
    
    public async Task AlterarFoto(AlterarFotoProdutoServicoDto dto)
    {
        if (dto.HaAlterar.Count != 5)
        {
            Notificator.Handle("Deve ser informado os 5 valores booleanos");
            return;
        }
        
        if (dto.Fotos.Count > 5)
        {
            Notificator.Handle("O número máximo de fotos permitidas é 5");
            return;
        }

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

        var arrayAux = links.ToString().Substring(1).Split("&");
        string[] fotosHaAdicionar = new string[5];
        for (int i = 0; i < arrayAux.Length; i++)
        {
            if (dto.HaAlterar[i])
            {
                fotosHaAdicionar[i] = arrayAux[i];
            }
        }
        
        var produtoServico = await _produtoServicoRepository.ObterPorId(dto.Id);
        if (produtoServico is null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        var arrayAuxProdutoServico = produtoServico.Foto.Substring(1).Split("&");
        string[] fotosProdutoServico = new string[5];
        for (int i = 0; i < arrayAuxProdutoServico.Length; i++)
        {
            fotosProdutoServico[i] = arrayAuxProdutoServico[i];
        }

        for (int i = 0; i < dto.HaAlterar.Count; i++)
        {
            if (dto.HaAlterar[i])
            {
                fotosProdutoServico[i] = fotosHaAdicionar[i];
            }
        }

        StringBuilder fotosString = new StringBuilder();
        foreach (var foto in fotosProdutoServico)
        {
            if (foto != null)
            {
                fotosString.Append("&" + foto);
            }
        }

        produtoServico.Foto = fotosString.ToString();
        _produtoServicoRepository.Alterar(produtoServico);
        if (await _produtoServicoRepository.UnitOfWork.Commit())
        {
            return;
        }
        
        Notificator.Handle("Não foi possível alterar as fotos");
    }

    public async Task RemoverFoto(RemoverFotosProdutoServicoDto dto)
    {
        if (dto.HaRemover.Count != 5)
        {
            Notificator.Handle("Deve ser informado os 5 valores booleanos");
            return;
        }
        
        var produtoServico = await _produtoServicoRepository.ObterPorId(dto.Id);
        if (produtoServico is null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        string[] fotosProdutoServico = new string[5];
        var fotosParaProdutoServico = produtoServico.Foto.Substring(1).Split("&");
        for (int i = 0; i < fotosParaProdutoServico.Length; i++)
        {
            fotosProdutoServico[i] = fotosParaProdutoServico[i];
        }

        for (int i = 0; i < dto.HaRemover.Count; i++)
        {
            if (dto.HaRemover[i])
            {
                fotosProdutoServico[i] = "";
            }
        }

        if (fotosProdutoServico.Length <= 0)
        {
            Notificator.Handle("O produto ou serviço deve conter ao menos uma foto!");
            return;
        }
        
        produtoServico.Foto = fotosProdutoServico.ToString()!;
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

        if (await _produtoServicoRepository.UnitOfWork.Commit())
        {
            _produtoServicoRepository.Remover(produtoServico);
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