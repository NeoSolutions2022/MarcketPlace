using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.Fornecedor;
using MarcketPlace.Application.Email;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Settings;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MarcketPlace.Application.Services;

public class FornecedorService : BaseService, IFornecedorService
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IPasswordHasher<Fornecedor> _passwordHasher;
    private readonly IEmailService _emailService;
    private readonly AppSettings _appSettings;

    public FornecedorService(IMapper mapper, INotificator notificator, IFornecedorRepository fornecedorRepository, IPasswordHasher<Fornecedor> passwordHasher, IOptions<AppSettings> appSettings, IEmailService emailService) : base(mapper, notificator)
    {
        _fornecedorRepository = fornecedorRepository;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
        _appSettings = appSettings.Value;
    }


    public async Task<PagedDto<FornecedorDto>> Buscar(BuscarFornecedorDto dto)
    {
        var fornecedor = await _fornecedorRepository.Buscar(dto);
        return Mapper.Map<PagedDto<FornecedorDto>>(fornecedor);
    }

    public async Task<FornecedorDto?> Cadastrar(CadastrarFornecedorDto dto)
    {
        if (dto.Senha != dto.ConfirmacaoSenha)
        {
            Notificator.Handle("As senhas não conferem!");
            return null;
        }
        
        var fornecedor = Mapper.Map<Fornecedor>(dto);
        if (!await Validar(fornecedor))
        {
            return null;
        }

        fornecedor.Senha = _passwordHasher.HashPassword(fornecedor, fornecedor.Senha);
        fornecedor.Uf = fornecedor.Uf.ToLower();
        _fornecedorRepository.Adicionar(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<FornecedorDto>(fornecedor);
        }
        
        Notificator.Handle("Não foi possível adicionar o fornecedor");
        return null;
    }

    public async Task<FornecedorDto?> Alterar(int id, AlterarFornecedorDto dto)
    {
        if (id != dto.Id)
        {
            Notificator.Handle("Os ids não conferem!");
            return null;
        }

        var fornecedor = await _fornecedorRepository.ObterPorId(id);
        if (fornecedor == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Mapper.Map(fornecedor, dto);
        if (!await Validar(fornecedor))
        {
            return null;
        }
        
        fornecedor.Senha = _passwordHasher.HashPassword(fornecedor, fornecedor.Senha);
        fornecedor.Uf = fornecedor.Uf.ToLower();
        _fornecedorRepository.Alterar(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            return Mapper.Map<FornecedorDto>(fornecedor);
        }
        
        Notificator.Handle("Não foi possível alterar o fornecedor");
        return null;
    }

    public async Task<FornecedorDto?> ObterPorId(int id)
    {
        var fornecedor = await _fornecedorRepository.ObterPorId(id);
        if (fornecedor != null)
        {
            return Mapper.Map<FornecedorDto>(fornecedor);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<FornecedorDto?> ObterPorEmail(string email)
    {
        var fornecedor = await _fornecedorRepository.ObterPorEmail(email);
        if (fornecedor != null)
        {
            return Mapper.Map<FornecedorDto>(fornecedor);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<FornecedorDto?> ObterPorCnpj(string cnpj)
    {
        var fornecedor = await _fornecedorRepository.ObterPorCnpj(cnpj);
        if (fornecedor != null)
        {
            return Mapper.Map<FornecedorDto>(fornecedor);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }
    
    public async Task<FornecedorDto?> ObterPorCpf(string cpf)
    {
        var fornecedor = await _fornecedorRepository.ObterPorCpf(cpf);
        if (fornecedor != null)
        {
            return Mapper.Map<FornecedorDto>(fornecedor);
        }
        
        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task ResetarSenha(int id)
    {
        var fornecedor = await _fornecedorRepository.FistOrDefault(f => f.Id == id);
        if (fornecedor == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        var codigoExpiraEmHoras = 3;
        fornecedor.CodigoResetarSenha = Guid.NewGuid();
        fornecedor.CodigoResetarSenhaExpiraEm = DateTime.Now.AddHours(codigoExpiraEmHoras);
        _fornecedorRepository.Alterar(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            _emailService.Enviar(
                fornecedor.Email,
                "Seu link para resetar a senha",
                "Usuario/CodigoResetarSenha",
                new
                {
                    Nome = fornecedor.Nome ?? fornecedor.Nome,
                    fornecedor.Email,
                    Codigo = fornecedor.CodigoResetarSenha,
                    Url = _appSettings.UrlComum,
                    ExpiracaoEmHoras = codigoExpiraEmHoras
                });
        }
    }

    public async Task Desativar(int id)
    {
        var fornecedor = await _fornecedorRepository.ObterPorId(id);
        if (fornecedor == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        fornecedor.Desativado = true;
        _fornecedorRepository.Alterar(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            return;
        }
        
        Notificator.Handle("Não foi possível desativar o fornecedor");
    }

    public async Task Reativar(int id)
    {
        var fornecedor = await _fornecedorRepository.ObterPorId(id);
        if (fornecedor == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        fornecedor.Desativado = false;
        _fornecedorRepository.Alterar(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            return;
        }
        
        Notificator.Handle("Não foi possível reativar o fornecedor");
    }

    private async Task<bool> Validar(Fornecedor fornecedor)
    {
        if (!fornecedor.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
        }

        var fornecedorExistente = await _fornecedorRepository.FistOrDefault(c =>
            (c.Email == fornecedor.Email || c.Cnpj == fornecedor.Cnpj) && c.Id != fornecedor.Id);

        if (fornecedorExistente != null)
        {
            Notificator.Handle("Já existe um fornecedor cadastrado com essas identificações");
        }

        return !Notificator.HasNotification;
    }
}