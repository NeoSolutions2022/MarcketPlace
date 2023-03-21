using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.Cliente;
using MarcketPlace.Application.Notification;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MarcketPlace.Application.Services;

public class ClienteService : BaseService, IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IPasswordHasher<Cliente> _passwordHasher;

    public ClienteService(IMapper mapper, INotificator notificator, IClienteRepository clienteRepository, IPasswordHasher<Cliente> passwordHasher) : base(mapper, notificator)
    {
        _clienteRepository = clienteRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<PagedDto<ClienteDto>> Buscar(BuscarClienteDto dto)
    {
        var cliente = await _clienteRepository.Buscar(dto);
        return Mapper.Map<PagedDto<ClienteDto>>(cliente);
    }

    public async Task<ClienteDto?> Cadastrar(CadastrarClienteDto dto)
    {
        var cliente = Mapper.Map<Cliente>(dto);
        if (!await Validar(cliente))
        {
            return null;
        }
        
        cliente.Senha = _passwordHasher.HashPassword(cliente, cliente.Senha);
        _clienteRepository.Adicionar(cliente);
        if (await _clienteRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<ClienteDto>(cliente);
        }
        
        Notificator.Handle("Não foi possível adicionar o cliente");
        return null;
    }

    public async Task<ClienteDto?> Alterar(int id, AlterarClienteDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não conferem!");
            return null;
        }

        var cliente = await _clienteRepository.ObterPorId(id);
        if (cliente == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(cliente, dto);
        if (!await Validar(cliente))
        {
            return null;
        }
        
        cliente.Senha = _passwordHasher.HashPassword(cliente, cliente.Senha);
        _clienteRepository.Alterar(cliente);
        if (await _clienteRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<ClienteDto>(cliente);
        }
        
        Notificator.Handle("Não foi possível alterar o cliente");
        return null;
    }

    public async Task<ClienteDto?> ObterPorId(int id)
    {
        var cliente = await _clienteRepository.ObterPorId(id);
        if (cliente != null)
        {
            return Mapper.Map<ClienteDto>(cliente);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<ClienteDto?> ObterPorEmail(string email)
    {
        var cliente = await _clienteRepository.ObterPorEmail(email);
        if (cliente != null)
        {
            return Mapper.Map<ClienteDto>(cliente);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<ClienteDto?> ObterPorCpf(string cpf)
    {
        var cliente = await _clienteRepository.ObterPorCpf(cpf);
        if (cliente != null)
        {
            return Mapper.Map<ClienteDto>(cliente);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task Desativar(int id)
    {
        var cliente = await _clienteRepository.ObterPorId(id);
        if (cliente == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        cliente.Desativado = true;
        _clienteRepository.Alterar(cliente);
        if (await _clienteRepository.UnitOfWork.Commit())
        {
            return;
        }
        
        Notificator.Handle("Não foi possível desativar o cliente");
    }

    public async Task Reativar(int id)
    {
        var cliente = await _clienteRepository.ObterPorId(id);
        if (cliente == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        cliente.Desativado = false;
        _clienteRepository.Alterar(cliente);
        if (await _clienteRepository.UnitOfWork.Commit())
        {
            return;
        }
        
        Notificator.Handle("Não foi possível reativar o cliente");
    }

    private async Task<bool> Validar(Cliente cliente)
    {
        if (!cliente.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }
        
        var clienteExistente = await _clienteRepository.FistOrDefault(c =>
            c.Cpf == cliente.Cpf || c.Email == cliente.Email && c.Id != cliente.Id);
        if (clienteExistente != null)
        {
            Notificator.Handle("Já existe um usuário cadastrador com uma ou mais identificações");
        }
        
        return !Notificator.HasNotification;
    }
}