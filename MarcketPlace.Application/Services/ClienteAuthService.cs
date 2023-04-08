using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Auth;
using MarcketPlace.Application.Dtos.V1.Cliente;
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

public class ClienteAuthService : BaseService, IClienteAuthService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IPasswordHasher<Cliente> _passwordHasher;
    private readonly IEmailService _emailService;
    private readonly AppSettings _appSettings;

    public ClienteAuthService(IMapper mapper, INotificator notificator, IClienteRepository clienteRepository,
        IPasswordHasher<Cliente> passwordHasher, IEmailService emailService, IOptions<AppSettings> appSettings) : base(
        mapper, notificator)
    {
        _clienteRepository = clienteRepository;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
        _appSettings = appSettings.Value;
    }

    public async Task<UsuarioAutenticadoDto?> Login(LoginDto loginDto)
    {
        var cliente = await _clienteRepository.ObterPorEmail(loginDto.Email);
        if (cliente == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(cliente, cliente.Senha, loginDto.Senha);
        if (result != PasswordVerificationResult.Failed)
        {
            return new UsuarioAutenticadoDto
            {
                Id = cliente.Id,
                Email = cliente.Email,
                Nome = cliente.Nome,
                Token = await CreateTokenCliente(cliente)
            };
        }

        Notificator.Handle("Combinação de email e senha incorreta!");
        return null;
    }

    public async Task<bool> VerificarCodigo(VerificarCodigoResetarSenhaClienteDto dto)
    {
        var cliente = await _clienteRepository.FistOrDefault(c =>
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

    public async Task RecuperarSenha(RecuperarSenhaClienteDto dto)
    {
        var cliente = await _clienteRepository.FistOrDefault(f => f.Email == dto.Email);
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
                    cliente.Nome,
                    cliente.Email,
                    Codigo = cliente.CodigoResetarSenha,
                    Url = _appSettings.UrlComum,
                    ExpiracaoEmHoras = codigoExpiraEmHoras
                });
        }
    }

    public async Task AlterarSenha(AlterarSenhaClienteDto dto)
    {
        var cliente = await _clienteRepository.FistOrDefault(c =>
            c.Email == dto.Email && c.CodigoResetarSenha == dto.CodigoResetarSenha);
        if (cliente == null)
        {
            Notificator.Handle("Cóidigo inválido ou expirado!");
            return;
        }

        if (!(cliente.CodigoResetarSenha == dto.CodigoResetarSenha &&
              cliente.CodigoResetarSenhaExpiraEm >= DateTime.Now))
        {
            Notificator.Handle("Código inválido ou expirado!");
            return;
        }

        if (!dto.Validar(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return;
        }

        cliente.Senha = _passwordHasher.HashPassword(cliente, dto.Senha);
        cliente.CodigoResetarSenha = null;
        cliente.CodigoResetarSenhaExpiraEm = null;

        _clienteRepository.Alterar(cliente);
        if (!await _clienteRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível alterar a senha");
        }
    }

    public Task<string> CreateTokenCliente(Cliente cliente)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                new Claim(ClaimTypes.Name, cliente.Nome),
                new Claim(ClaimTypes.Email, cliente.Email),
                new Claim("TipoUsuario", ETipoUsuario.Cliente.ToDescriptionString()),
                new Claim("Administrador", ETipoUsuario.Cliente.ToDescriptionString()),
                new Claim("Fornecedor", ETipoUsuario.Cliente.ToDescriptionString()),
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