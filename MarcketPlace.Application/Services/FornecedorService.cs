﻿using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Application.Dtos.V1.Fornecedor;
using MarcketPlace.Application.Email;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Enums;
using MarcketPlace.Core.Extensions;
using MarcketPlace.Core.Settings;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MarcketPlace.Application.Services;

public class FornecedorService : BaseService, IFornecedorService
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IPasswordHasher<Fornecedor> _passwordHasher;
    private readonly IEmailService _emailService;
    private readonly AppSettings _appSettings;
    private readonly IFileService _fileService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FornecedorService(IMapper mapper, INotificator notificator, IFornecedorRepository fornecedorRepository,
        IPasswordHasher<Fornecedor> passwordHasher, IOptions<AppSettings> appSettings, IEmailService emailService,
        IFileService fileService, IHttpContextAccessor httpContextAccessor) : base(mapper, notificator)
    {
        _fornecedorRepository = fornecedorRepository;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
        _fileService = fileService;
        _httpContextAccessor = httpContextAccessor;
        _appSettings = appSettings.Value;
    }


    public async Task<PagedDto<FornecedorDto>> Buscar(BuscarFornecedorDto dto)
    {
        var fornecedor = await _fornecedorRepository.Buscar(dto);
        return Mapper.Map<PagedDto<FornecedorDto>>(fornecedor);
    }

    public async Task<PagedDto<FornecedorDto>> BuscarAnuncio()
    {
        var usuarioLogado =
            await _fornecedorRepository.ObterPorId(
                Convert.ToInt32(_httpContextAccessor?.HttpContext?.User.ObterUsuarioId()));
        if (usuarioLogado == null)
        {
            Notificator.Handle("Não foi possível identificar o usuário logado");
        }

        var fornecedor = await _fornecedorRepository.Buscar(new BuscarFornecedorDto
            { Cidade = usuarioLogado?.Cidade, AnuncioPago = true });

        if (fornecedor.Itens.Count == 0)
        {
            return Mapper.Map<PagedDto<FornecedorDto>>(await _fornecedorRepository.Buscar(new BuscarFornecedorDto{AnuncioPago = true}));
        }

        return Mapper.Map<PagedDto<FornecedorDto>>(fornecedor);
    }

    public async Task<FornecedorDto?> Cadastrar(CadastrarFornecedorDto dto)
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

        var fornecedor = Mapper.Map<Fornecedor>(dto);
        if (!await Validar(fornecedor))
        {
            return null;
        }

        if (dto.Foto is { Length: > 0 })
        {
            fornecedor.Foto = await _fileService.Upload(dto.Foto, EUploadPath.FotoFornecedor);
        }

        fornecedor.Senha = _passwordHasher.HashPassword(fornecedor, fornecedor.Senha);
        fornecedor.Uf = fornecedor.Uf.ToLower();
        fornecedor.CriadoEm = DateTime.Now;
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

        Mapper.Map(dto, fornecedor);
        if (!await Validar(fornecedor))
        {
            return null;
        }

        fornecedor.Uf = fornecedor.Uf.ToLower();
        fornecedor.AtualizadoEm = DateTime.Now;
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


    public async Task AlterarSenha(int id)
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
                "Seu link para alterar a senha",
                "Usuario/CodigoResetarSenha",
                new
                {
                    fornecedor.Nome,
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
        fornecedor.AtualizadoEm = DateTime.Now;
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
        fornecedor.AtualizadoEm = DateTime.Now;
        _fornecedorRepository.Alterar(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível reativar o fornecedor");
    }

    public async Task AtivarAnuncio(int id)
    {
        var fornecedor = await _fornecedorRepository.ObterPorId(id);
        if (fornecedor == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        fornecedor.AnuncioPago = true;
        fornecedor.DataPagamentoAnuncio = DateTime.Now;
        fornecedor.DataExpiracaoAnuncio = fornecedor.DataPagamentoAnuncio.Value.AddMonths(1);
        fornecedor.AtualizadoEm = DateTime.Now;
        _fornecedorRepository.Alterar(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível ativar o anúncio para o fornecedor");
    }

    public async Task DesativarAnuncio(int id)
    {
        var fornecedor = await _fornecedorRepository.ObterPorId(id);
        if (fornecedor == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        fornecedor.AnuncioPago = false;
        fornecedor.AtualizadoEm = DateTime.Now;
        _fornecedorRepository.Alterar(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível desativar o anúncio para o fornecedor");
    }

    public async Task AlterarDescricao(int id, string descricao)
    {
        var fornecedor = await _fornecedorRepository.ObterPorId(id);
        if (fornecedor == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        fornecedor.Descricao = descricao;
        _fornecedorRepository.Alterar(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível reativar o fornecedor");
    }

    public async Task AlterarFoto(int id, AlterarFotoFornecedorDto foto)
    {
        var fornecedor = await _fornecedorRepository.ObterPorId(id);
        if (fornecedor == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        fornecedor.Foto = await _fileService.Upload(foto.Foto, EUploadPath.FotoFornecedor);
        _fornecedorRepository.Alterar(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível reativar o fornecedor");
    }

    public async Task Remover(int id)
    {
        var fornecedor = await _fornecedorRepository.FistOrDefault(c => c.Id == id);
        if (fornecedor == null)
        {
            Notificator.Handle("Não existe um fornecedor com o id informado");
            return;
        }

        _fornecedorRepository.Remover(fornecedor);
        if (await _fornecedorRepository.UnitOfWork.Commit())
        {
            return;
        }

        Notificator.Handle("Não foi possível remover o fornecedor");
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

    private bool ValidarAnexos(CadastrarFornecedorDto dto)
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
}