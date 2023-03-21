namespace MarcketPlace.Application.Dtos.V1.Fornecedor;

public class FornecedorDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? NomeSocial { get; set; }
    public string Email { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string? Cnpj { get; set; }
    public string? Telefone { get; set; }
    public bool Desativado { get; set; }
}