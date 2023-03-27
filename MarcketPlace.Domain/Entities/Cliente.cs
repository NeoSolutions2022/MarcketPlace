using FluentValidation.Results;
using MarcketPlace.Domain.Contracts;
using MarcketPlace.Domain.Validation;

namespace MarcketPlace.Domain.Entities;

public class Cliente : Entity, ISoftDelete, IAggregateRoot
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
    public Guid? CodigoResetarSenha { get; set; }
    public DateTime? CodigoResetarSenhaExpiraEm { get; set; }
    public string Cep { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Uf { get; set; } = null!;
    public string Endereco { get; set; } = null!;
    public int Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Foto { get; set; }

    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new ClienteValidator().Validate(this);
        return validationResult.IsValid;
    }
}