using FluentValidation;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Validation;

public class ClienteValidator: AbstractValidator<Cliente>
{
    public ClienteValidator()
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

        RuleFor(u => u.DataPagamento)
            .NotEmpty();

        RuleFor(u => u.Desativado)
            .NotNull();
    }
}