namespace MarcketPlace.Application.Dtos.V1.Cliente;

public class VerificarCodigoResetarSenhaClienteDto
{
    public string Email { get; set; } = null!;
    public Guid CodigoResetarSenha { get; set; }
}