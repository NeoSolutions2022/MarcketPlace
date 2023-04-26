using FluentValidation;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Validation;

public class CaracteristicaProdutoServicoValidator : AbstractValidator<CaracteristicaProdutoServico>
{
    public CaracteristicaProdutoServicoValidator()
    {
        // RuleFor(c => c.Chave)
        //     .
    }
}