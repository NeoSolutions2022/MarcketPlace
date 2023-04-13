namespace MarcketPlace.Application.Dtos.V1.Administrador;

public class VerificarCodigoResetarSenhaAdministradorDto
{
    public string Email { get; set; } = null!;
    public Guid CodigoResetarSenha { get; set; }
}