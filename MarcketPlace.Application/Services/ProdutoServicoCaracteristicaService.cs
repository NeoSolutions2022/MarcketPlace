using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.ProdutoServico.ProdutoServicoCaracteristica;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Extensions;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MarcketPlace.Application.Services;

public class ProdutoServicoCaracteristicaService : BaseService, IProdutoServicoCaracteristicaService
{
    private readonly IProdutoServicoCaracteristicaRepository _repository;
    private readonly HttpContextAccessor _httpContextAccessor;
    public ProdutoServicoCaracteristicaService(IMapper mapper, INotificator notificator, IProdutoServicoCaracteristicaRepository repository, IOptions<HttpContextAccessor> httpContextAccessor) : base(mapper, notificator)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor.Value;
    }

    public async Task<List<ProdutoServicoCaracteristicaDto>?> Adicionar(List<AdicionarProdutoServicoCaracteristicaDto> dto)
    {
        var produtoServicoCaracteristica = Mapper.Map<List<ProdutoServicoCaracteristica>>(dto);
        foreach (var produto in produtoServicoCaracteristica)
        {
            if (!await Validar(produto))
            {
                return null;
            }
        }

        _repository.Adicionar(produtoServicoCaracteristica);

        if (await _repository.UnitOfWork.Commit())
        {
            return Mapper.Map<List<ProdutoServicoCaracteristicaDto>>(produtoServicoCaracteristica);
        }

        Notificator.Handle("Não foi possível salvar o produto ou serviço!");
        return null;
    }

    public async Task<ProdutoServicoCaracteristicaDto?> Alterar(int id, AlterarProdutoServicoCaracteristicaDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não conferem!");
            return null;
        }

        var produtoServicoCaracteristica = await _repository.ObterPorId(id);
        if (produtoServicoCaracteristica == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(dto, produtoServicoCaracteristica);
        if (!await Validar(produtoServicoCaracteristica))
        {
            return null;
        }

        _repository.Alterar(produtoServicoCaracteristica);
        if (await _repository.UnitOfWork.Commit())
        {
            return Mapper.Map<ProdutoServicoCaracteristicaDto>(produtoServicoCaracteristica);
        }

        Notificator.Handle("Não foi possível alterar o produto ou serviço!");
        return null;
    }

    public async Task<ProdutoServicoCaracteristicaDto?> ObterPorId(int id)
    {
        var produtoServicoCaracteristica = await _repository.ObterPorId(id);
        if (produtoServicoCaracteristica != null)
        {
            return Mapper.Map<ProdutoServicoCaracteristicaDto>(produtoServicoCaracteristica);
        }

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<ProdutoServicoCaracteristicaDto>?> ObterPorTodos(int produtoId)
    {
        var produtoServicoCaracteristica = await _repository.ObterTodos(produtoId);
        if (produtoServicoCaracteristica != null)
        {
            return Mapper.Map<List<ProdutoServicoCaracteristicaDto>>(produtoServicoCaracteristica);
        }

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task Remover(int id)
    {
        var produtoServicoCaracteristica = await _repository.ObterPorId(id);
        if (produtoServicoCaracteristica is null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        _repository.Remover(produtoServicoCaracteristica);
        if (await _repository.UnitOfWork.Commit())
        {
            _repository.Remover(produtoServicoCaracteristica);
            return;
        }

        Notificator.Handle("Não foi possível remover o produto ou serviço");
    }
    
    private async Task<bool> Validar(ProdutoServicoCaracteristica produtoCaracteristica)
    {
        if (!produtoCaracteristica.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }
            
        var caracteristicaExistente =
            await _repository.FistOrDefault(
                c => c.Chave == produtoCaracteristica.Chave && c.Id != produtoCaracteristica.Id);
        if (caracteristicaExistente != null)
        {
            Notificator.Handle("Já existe uma caracteristica com essa chave!");
        }

        if (caracteristicaExistente != null && caracteristicaExistente.ProdutoServico.FornecedorId != Convert.ToInt32(_httpContextAccessor.HttpContext?.User.ObterUsuarioId()))
        {
            Notificator.Handle("Você não tem permição para executar essa ação!");
        }

        return !Notificator.HasNotification;
    }
}