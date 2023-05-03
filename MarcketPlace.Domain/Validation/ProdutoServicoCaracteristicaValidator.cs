using FluentValidation;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Validation;

public class ProdutoServicoCaracteristicaValidator : AbstractValidator<ProdutoServicoCaracteristica>
{
    public ProdutoServicoCaracteristicaValidator()
    {
        RuleFor(c => c.Valor)
            .MaximumLength(255);
        
        RuleFor(c => c.Chave)
            .MaximumLength(50);
    }
}