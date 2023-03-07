using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Administrador;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Notification;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MarcketPlace.Application.Services;

public class AdministradorService : BaseService, IAdministradorService
{
    private readonly IAdministradorRepository _administradorRepository;
    private readonly IPasswordHasher<Administrador> _passwordHasher;

    public AdministradorService(IMapper mapper, INotificator notificator, IAdministradorRepository administradorRepository, IPasswordHasher<Administrador> passwordHasher) : base(mapper, notificator)
    {
        _administradorRepository = administradorRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<PagedDto<AdministradorDto>> Buscar(BuscarAdministradorDto dto)
    {
        var administrador = await _administradorRepository.Buscar(dto);
        return Mapper.Map<PagedDto<AdministradorDto>>(administrador);
    }

    public async Task<AdministradorDto?> Adicionar(AdicionarAdministradorDto dto)
    {
        var administrador = Mapper.Map<Administrador>(dto);
        if (!await Validar(administrador))
        {
            return null;
        }
        
        administrador.Senha = _passwordHasher.HashPassword(administrador, administrador.Senha);
        _administradorRepository.Adicionar(administrador);
        if (await _administradorRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<AdministradorDto>(administrador);
        }
        
        Notificator.Handle("Não foi possível adicionar o administrador");
        return null;
    }

    public async Task<AdministradorDto?> Alterar(int id, AlterarAdministradorDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não conferem!");
            return null;
        }

        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(administrador, dto);
        if (!await Validar(administrador))
        {
            return null;
        }
        
        administrador.Senha = _passwordHasher.HashPassword(administrador, administrador.Senha);
        _administradorRepository.Alterar(administrador);
        if (await _administradorRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<AdministradorDto>(administrador);
        }
        
        Notificator.Handle("Não foi possível alterar o administrador");
        return null;
    }

    public async Task<AdministradorDto?> ObterPorId(int id)
    {
        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador != null)
        {
            return Mapper.Map<AdministradorDto>(administrador);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<AdministradorDto?> ObterPorEmail(string email)
    {
        var administrador = await _administradorRepository.ObterPorEmail(email);
        if (administrador != null)
        {
            return Mapper.Map<AdministradorDto>(administrador);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task Desaticar(int id)
    {
        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        administrador.Desativado = true;
        _administradorRepository.Alterar(administrador);
        if (await _administradorRepository.UnitOfWork.Commit())
        {
            return;
        }
        
        Notificator.Handle("Não foi possível desativar o administrador");
    }
    
    public async Task Reativar(int id)
    {
        var administrador = await _administradorRepository.ObterPorId(id);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        administrador.Desativado = false;
        _administradorRepository.Alterar(administrador);
        if (await _administradorRepository.UnitOfWork.Commit())
        {
            return;
        }
        
        Notificator.Handle("Não foi possível reativar o administrador");
    }

    private async Task<bool> Validar(Administrador administrador)
    {
        if (!administrador.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }

        var administradorExistente =
            await _administradorRepository.FistOrDefault(
                c => c.Email == administrador.Email && c.Id != administrador.Id);
        if (administradorExistente != null)
        {
            Notificator.Handle("Já existe um administrador com esse email!");
        }

        return !Notificator.HasNotification;
    }
}