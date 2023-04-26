using FluentValidation;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Validation;

public class ProdutoServicoCaracteristicaValidator : AbstractValidator<ProdutoServicoCaracteristica>
{
    public ProdutoServicoCaracteristicaValidator()
    {
        RuleFor(c => c.Chave)
            .MaximumLength(100);
        
        RuleFor(c => c.Valor)
            .MaximumLength(255);
    }
}