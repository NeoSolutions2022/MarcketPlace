namespace MarcketPlace.Application.Dtos.V1.Fornecedor;

public class VerificarCodigoResetarSenhaFornecedorDto
{
    public string Email { get; set; } = null!;
    public Guid CodigoResetarSenha { get; set; }
}