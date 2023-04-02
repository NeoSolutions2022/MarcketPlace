using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Dtos.V1.Fornecedor;

public class CadastrarFornecedorDto
{
    public string Nome { get; set; } = null!;
    public string Cep { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Endereco { get; set; } = null!;
    public int Numero { get; set; }
    public string Uf { get; set; } = null!;
    public string? Cnpj { get; set; }
    public string? Complemento { get; set; }
    public string Cpf { get; set; } = null!;
    public bool Desativado { get; set; }
    public string Email { get; set; } = null!;
    public string Responsavel { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string? Telefone { get; set; }
    [Required(ErrorMessage = "A confirmação da senha é necessária")]
    public string ConfirmacaoSenha { get; set; } = null!;

    public IFormFile? Foto { get; set; }
}