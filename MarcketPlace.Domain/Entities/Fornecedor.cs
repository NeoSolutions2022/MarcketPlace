using FluentValidation.Results;
using MarcketPlace.Domain.Contracts;
using MarcketPlace.Domain.Validation;

namespace MarcketPlace.Domain.Entities;

public class Fornecedor : Entity, IAggregateRoot, ISoftDelete
{
    public string Nome { get; set; } = null!;
    public string? NomeSocial { get; set; }
    public string Email { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string? Cnpj { get; set; }
    public string? Telefone { get; set; }
    public string Senha { get; set; } = null!;
    public bool Desativado { get; set; }
    public Guid? CodigoResetarSenha { get; set; }
    public DateTime? CodigoResetarSenhaExpiraEm { get; set; }

    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new FornecedorValidator().Validate(this);
        return validationResult.IsValid;
    }
}