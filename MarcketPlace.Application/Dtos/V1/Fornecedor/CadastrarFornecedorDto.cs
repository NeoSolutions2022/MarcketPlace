namespace MarcketPlace.Application.Dtos.V1.Fornecedor;

public class CadastrarFornecedorDto
{
    public string Nome { get; set; } = null!;
    public string? NomeSocial { get; set; }
    public string Email { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string? Cnpj { get; set; }
    public string? Telefone { get; set; }
    public string Senha { get; set; } = null!;
    public bool Desativado { get; set; }
}