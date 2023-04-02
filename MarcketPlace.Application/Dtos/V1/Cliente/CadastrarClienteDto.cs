using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Dtos.V1.Cliente;

public class CadastrarClienteDto
{
    public string Nome { get; set; } = null!;
    public string? NomeSocial { get; set; }
    public string Email { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string? Telefone { get; set; }
    public string Senha { get; set; } = null!;
    [Required(ErrorMessage = "A confirmação da senha é necessária")]
    public string ConfirmacaoSenha { get; set; } = null!;
    public bool? Inadiplente { get; set; }
    public DateTime DataPagamento { get; set; }
    public bool Desativado { get; set; }
    public string Cep { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Uf { get; set; } = null!;
    public string Endereco { get; set; } = null!;
    public int Numero { get; set; }
    public string? Complemento { get; set; }
    public IFormFile? Foto { get; set; }
}