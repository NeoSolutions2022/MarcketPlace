using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Dtos.V1.Auth;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Enums;
using MarcketPlace.Core.Extensions;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MarcketPlace.Application.Services;

public class UsuarioAuthService : BaseService, IUsuarioAuthService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IPasswordHasher<Cliente> _clientepasswordHasher;
    private readonly IPasswordHasher<Fornecedor> _fornecedorpasswordHasher;

    public UsuarioAuthService(IMapper mapper, INotificator notificator, IFornecedorRepository fornecedorRepository, IClienteRepository clienteRepository, IPasswordHasher<Cliente> clientepasswordHasher, IPasswordHasher<Fornecedor> fornecedorpasswordHasher) : base(mapper, notificator)
    {
        _fornecedorRepository = fornecedorRepository;
        _clienteRepository = clienteRepository;
        _clientepasswordHasher = clientepasswordHasher;
        _fornecedorpasswordHasher = fornecedorpasswordHasher;
    }

    public async Task<UsuarioAutenticadoDto?> LoginCliente(LoginDto loginDto)
    {
        var cliente = await _clienteRepository.ObterPorEmail(loginDto.Email);
        if (cliente == null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        var result = _clientepasswordHasher.VerifyHashedPassword(cliente, cliente.Senha, loginDto.Senha);
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

    public async Task<UsuarioAutenticadoDto?> LoginFornecedor(LoginDto loginDto)
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
    
    public Task<string> CreateTokenCliente(Cliente cliente)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                new Claim(ClaimTypes.Name, cliente.Nome),
                new Claim(ClaimTypes.Email, cliente.Email),
                new Claim("TipoUsuario", ETipoUsuario.Comum.ToDescriptionString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
    
    public Task<string> CreateTokenFornecedor(Fornecedor fornecedor)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Settings.Settings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, fornecedor.Id.ToString()),
                new Claim(ClaimTypes.Name, fornecedor.Nome),
                new Claim(ClaimTypes.Email, fornecedor.Email),
                new Claim("TipoUsuario", ETipoUsuario.Comum.ToDescriptionString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}