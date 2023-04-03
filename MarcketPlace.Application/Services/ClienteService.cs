using System.Reflection.Metadata;
using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.Cliente;
using MarcketPlace.Application.Email;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Enums;
using MarcketPlace.Core.Settings;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MarcketPlace.Application.Services;

public class ClienteService : BaseService, IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IPasswordHasher<Cliente> _passwordHasher;
    private readonly IFileService _fileService;
    private readonly AppSettings _appSettings;
    private readonly IEmailService _emailService;

    public ClienteService(IMapper mapper, INotificator notificator, IClienteRepository clienteRepository, IPasswordHasher<Cliente> passwordHasher, IFileService fileService, IOptions<AppSettings> appSettings, IEmailService emailService) : base(mapper, notificator)
    {
        _clienteRepository = clienteRepository;
        _passwordHasher = passwordHasher;
        _fileService = fileService;
        _emailService = emailService;
        _appSettings = appSettings.Value;
    }

    public async Task<PagedDto<ClienteDto>> Buscar(BuscarClienteDto dto)
    {
        var cliente = await _clienteRepository.Buscar(dto);
        return Mapper.Map<PagedDto<ClienteDto>>(cliente);
    }

    public async Task<ClienteDto?> Cadastrar(CadastrarClienteDto dto)
    {
        if (!ValidarAnexos(dto))
        {
            return null;
        }
        
        if (dto.Senha != dto.ConfirmacaoSenha)
        {
            Notificator.Handle("As senhas não conferem!");
            return null;
        }
        
        var cliente = Mapper.Map<Cliente>(dto);
        if (!await Validar(cliente))
        {
            return null;
        }

        if (dto.Foto is {Length: > 0})
        {
            cliente.Foto = await _fileService.Upload(dto.Foto, EUploadPath.FotoFornecedor);
        }
        
        cliente.Senha = _passwordHasher.HashPassword(cliente, cliente.Senha);
        cliente.Uf = cliente.Uf.ToLower();
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
        
        if (dto.Foto is { Length: > 0 } && !await ManterFoto(dto.Foto, cliente))
        {
            return null;
        }
        
        cliente.Senha = _passwordHasher.HashPassword(cliente, cliente.Senha);
        cliente.Uf = cliente.Uf.ToLower();
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

    public async Task AlterarSenha(int id)
    {
        var cliente = await _clienteRepository.FistOrDefault(f => f.Id == id);
        if (cliente == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        var codigoExpiraEmHoras = 3;
        cliente.CodigoResetarSenha = Guid.NewGuid();
        cliente.CodigoResetarSenhaExpiraEm = DateTime.Now.AddHours(codigoExpiraEmHoras);
        _clienteRepository.Alterar(cliente);
        if (await _clienteRepository.UnitOfWork.Commit())
        {
            _emailService.Enviar(
                cliente.Email,
                "Seu link para alterar a senha",
                "Usuario/CodigoResetarSenha",
                new
                {
                    Nome = cliente.NomeSocial ?? cliente.Nome,
                    cliente.Email,
                    Codigo = cliente.CodigoResetarSenha,
                    Url = _appSettings.UrlComum,
                    ExpiracaoEmHoras = codigoExpiraEmHoras
                });
        }
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

    public async Task Remover(int id)
    {
        var cliente = await _clienteRepository.FistOrDefault(c => c.Id == id);
        if (cliente == null)
        {
            Notificator.Handle("Não existe um cliente com o id informado");
            return;
        }
        
        _clienteRepository.Remover(cliente);
        if (await _clienteRepository.UnitOfWork.Commit())
        {
            return;
        }
        
        Notificator.Handle("Não foi possível remover o cliente");
        return;
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
    
    private bool ValidarAnexos(CadastrarClienteDto dto)
    {
        if (dto.Foto?.Length > 10000000)
        {
            Notificator.Handle("Foto deve ter no máximo 10Mb");
        }

        if (dto.Foto != null && dto.Foto.FileName.Split(".").Last() != "jfif" &&
            dto.Foto.FileName.Split(".").Last() != "png" && dto.Foto.FileName.Split(".").Last() != "jpg" 
            && dto.Foto.FileName.Split(".").Last() != "jpeg")
        {
            Notificator.Handle("Foto deve do tipo png, jfif ou jpg");
        }

        return !Notificator.HasNotification;
    }
    
    private async Task<bool> ManterFoto(IFormFile foto, Cliente cliente)
    {
        cliente.Foto = await _fileService.Upload(foto, EUploadPath.FotoCliente);
        return true;
    }
}