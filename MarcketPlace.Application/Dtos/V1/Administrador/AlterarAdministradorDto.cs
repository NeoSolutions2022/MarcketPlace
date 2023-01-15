namespace MarcketPlace.Application.Dtos.V1.Administrador;

public class AlterarAdministradorDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public bool Desativado { get; set; }
}