using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Administrador;
using MarcketPlace.Application.Dtos.V1.Auth;
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

public class AuthService : BaseService, IAuthService
 {
    private readonly IAdministradorRepository _administradorRepository;
    private readonly IPasswordHasher<Administrador> _admPasswordHasher;
    private readonly IEmailService _emailService;
    private readonly AppSettings _appSettings;
    private readonly IPasswordHasher<Administrador> _passwordHasher;

    public AuthService(IMapper mapper, INotificator notificator, IAdministradorRepository administradorRepository,
        IPasswordHasher<Administrador> admPasswordHasher, IEmailService emailService, IOptions<AppSettings> appSettings, IPasswordHasher<Administrador> passwordHasher) : base(mapper, notificator)
    {
        _administradorRepository = administradorRepository;
        _admPasswordHasher = admPasswordHasher;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
        _appSettings = appSettings.Value;
    }

    public async Task<AdministradorAutenticadoDto?> LoginAdministrador(LoginDto loginDto)
    {
        var administrador = await _administradorRepository.ObterPorEmail(loginDto.Email);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        var result = _admPasswordHasher.VerifyHashedPassword(administrador, administrador.Senha, loginDto.Senha);
        if (result != PasswordVerificationResult.Failed)
        {
            return new AdministradorAutenticadoDto
            {
                Id = administrador.Id,
                Email = administrador.Email,
                Nome = administrador.Nome,
                Token = await CreateToken(administrador)
            };
        }

        Notificator.Handle("Combinação de email e senha incorreta!");
        return null;
    }

    public async Task<bool> VerificarCodigo(VerificarCodigoResetarSenhaAdministradorDto administradorDto)
    {
        var administrador = await _administradorRepository.FistOrDefault(c =>
            c.Email == administradorDto.Email && c.CodigoResetarSenha == administradorDto.CodigoResetarSenha);
        if (administrador == null)
        {
            Notificator.Handle("Código inválido ou expirado!");
            return false;
        }

        if (administrador.CodigoResetarSenha == administradorDto.CodigoResetarSenha && administrador.CodigoResetarSenhaExpiraEm >= DateTime.Now)
        {
            return true;
        }

        Notificator.Handle("Código inválido ou expirado!");
        return false;
    }

    public async Task RecuperarSenha(RecuperarSenhaAdministradorDto dto)
    {
        var administrador = await _administradorRepository.FistOrDefault(f => f.Email == dto.Email);
        if (administrador == null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }

        var codigoExpiraEmHoras = 3;
        administrador.CodigoResetarSenha = Guid.NewGuid();
        administrador.CodigoResetarSenhaExpiraEm = DateTime.Now.AddHours(codigoExpiraEmHoras);
        _administradorRepository.Alterar(administrador);
        if (await _administradorRepository.UnitOfWork.Commit())
        {
            _emailService.Enviar(
                administrador.Email,
                "Seu link para alterar a senha",
                "Usuario/CodigoResetarSenha",
                new
                {
                    administrador.Nome,
                    administrador.Email,
                    Codigo = administrador.CodigoResetarSenha,
                    Url = _appSettings.UrlComum,
                    ExpiracaoEmHoras = codigoExpiraEmHoras
                });
        }
    }

    public async Task AlterarSenha(AlterarSenhaAdministradorDto dto)
    {
        var administrador = await _administradorRepository.FistOrDefault(c =>
            c.Email == dto.Email && c.CodigoResetarSenha == dto.CodigoResetarSenha);
        if (administrador == null)
        {
            Notificator.Handle("Cóidigo inválido ou expirado!");
            return;
        }

        if (!(administrador.CodigoResetarSenha == dto.CodigoResetarSenha &&
              administrador.CodigoResetarSenhaExpiraEm >= DateTime.Now))
        {
            Notificator.Handle("Código inválido ou expirado!");
            return;
        }

        if (!dto.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return;
        }

        administrador.Senha = _passwordHasher.HashPassword(administrador, dto.Senha);
        administrador.CodigoResetarSenha = null;
        administrador.CodigoResetarSenhaExpiraEm = null;

        _administradorRepository.Alterar(administrador);
        if (!await _administradorRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível alterar a senha");
        }
    }

    public Task<string> CreateToken(Administrador administrador)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, administrador.Id.ToString()),
                new Claim(ClaimTypes.Name, administrador.Nome),
                new Claim(ClaimTypes.Email, administrador.Email),
                new Claim("TipoUsuario", ETipoUsuario.Administrador.ToDescriptionString()),
                new Claim("Administrador", ETipoUsuario.Administrador.ToDescriptionString()),
                new Claim("Fornecedor", ETipoUsuario.Fornecedor.ToDescriptionString()),
                new Claim("Cliente", ETipoUsuario.Cliente.ToDescriptionString()),
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}