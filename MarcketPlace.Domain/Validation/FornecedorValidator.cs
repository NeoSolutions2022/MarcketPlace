using FluentValidation;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Validation;

public class FornecedorValidator : AbstractValidator<Fornecedor>
{
    public FornecedorValidator()
    {
        RuleFor(u => u.Nome)
            .NotEmpty();

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Cpf)
            .NotEmpty();
        
        RuleFor(u => u.Senha)
            .NotEmpty();

        RuleFor(u => u.Desativado)
            .NotNull();
    }
}