namespace MarcketPlace.Application.Dtos.V1.Cliente;

public class CadastrarClienteDto
{
    public string Nome { get; set; } = null!;
    public string? NomeSocial { get; set; }
    public string Email { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string? Telefone { get; set; }
    public string Senha { get; set; } = null!;
    public bool? Inadiplente { get; set; }
    public DateTime DataPagamento { get; set; }
    public bool Desativado { get; set; }
}