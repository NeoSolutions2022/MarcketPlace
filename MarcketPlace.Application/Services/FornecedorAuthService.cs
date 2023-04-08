using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Auth;
using MarcketPlace.Application.Dtos.V1.Fornecedor;
using MarcketPlace.Application.Email;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Enums;
using MarcketPlace.Core.Extensions;
using MarcketPlace.Core.Settings;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MarcketPlace.Application.Services;

public class FornecedorAuthService : BaseService, IFornecedorAuthService
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IPasswordHasher<Fornecedor> _fornecedorpasswordHasher;
    private readonly IEmailService _emailService;
    private readonly AppSettings _appSettings;

    public FornecedorAuthService(IMapper mapper, INotificator notificator, IFornecedorRepository fornecedorRepository,
        IPasswordHasher<Fornecedor> fornecedorpasswordHasher, IEmailService emailService,
        IOptions<AppSettings> appSettings) : base(mapper, notificator)
    {
        _fornecedorRepository = fornecedorRepository;
        _fornecedorpasswordHasher = fornecedorpasswordHasher;
        _emailService = emailService;
        _appSettings = appSettings.Value;
    }

    public async Task<UsuarioAutenticadoDto?> Login(LoginDto loginDto)
    {
        var fornecedor = await _fornecedorRepository.ObterPorEmail(loginDto.Email);
        if (fornecedor == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        var result = _fornecedorpasswordHasher.VerifyHashedPassword(fornecedor, fornecedor.Senha, loginDto.Senha);
        if (result != PasswordVerificationResult.Failed)
        {
            return new UsuarioAutenticadoDto
            {
                Id = fornecedor.Id,
                Email = fornecedor.Email,
                Nome = fornecedor.Nome,
                Token = await CreateTokenFornecedor(fornecedor)
            };
        }

        Notificator.Handle("Combinação de email e senha incorreta!");
        return null;
    }

    public async Task<bool> VerificarCodigo(VerificarCodigoResetarSenhaFornecedorDto dto)
    {
        var cliente = await _fornecedorRepository.FistOrDefault(c =>
            c.Email == dto.Email && c.CodigoResetarSenha == dto.CodigoResetarSenha);
        if (cliente == null)
        {
            Notificator.Handle("Código inválido ou expirado!");
            return false;
        }

        if (cliente.CodigoResetarSenha == dto.CodigoResetarSenha && cliente.CodigoResetarSenhaExpiraEm >= DateTime.Now)
        {
            return true;
        }

        Notificator.Handle("Código inválido ou expirado!");
        return false;
    }

    public async Task RecuperarSenha(RecuperarSenhaFornecedorDto dto)
    {
        var fornecedor = await _fornecedorRepository.FistOrDefault(f => f.Email == dto.Email);
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

    public async Task AlterarSenha(AlterarSenhaFornecedorDto dto)
    {
        var fornecedor = await _fornecedorRepository.FistOrDefault(c =>
            c.Email == dto.Email && c.CodigoResetarSenha == dto.CodigoResetarSenha);
        if (fornecedor == null)
        {
            Notificator.Handle("Cóidigo inválido ou expirado!");
            return;
        }

        if (!(fornecedor.CodigoResetarSenha == dto.CodigoResetarSenha &&
              fornecedor.CodigoResetarSenhaExpiraEm >= DateTime.Now))
        {
            Notificator.Handle("Código inválido ou expirado!");
            return;
        }

        if (!dto.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return;
        }

        fornecedor.Senha = _fornecedorpasswordHasher.HashPassword(fornecedor, dto.Senha);
        fornecedor.CodigoResetarSenha = null;
        fornecedor.CodigoResetarSenhaExpiraEm = null;

        _fornecedorRepository.Alterar(fornecedor);
        if (!await _fornecedorRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível alterar a senha");
        }
    }

    public Task<string> CreateTokenFornecedor(Fornecedor fornecedor)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, fornecedor.Id.ToString()),
                new Claim(ClaimTypes.Name, fornecedor.Nome),
                new Claim(ClaimTypes.Email, fornecedor.Email),
                new Claim("TipoUsuario", ETipoUsuario.Fornecedor.ToDescriptionString()),
                new Claim("Administrador", ETipoUsuario.Fornecedor.ToDescriptionString()),
                new Claim("Fornecedor", ETipoUsuario.Fornecedor.ToDescriptionString()),
                new Claim("Cliente", ETipoUsuario.Fornecedor.ToDescriptionString()),
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}