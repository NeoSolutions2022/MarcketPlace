using FluentValidation;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Validation;

public class ProdutoServicoValidator : AbstractValidator<ProdutoServico>
{
    public ProdutoServicoValidator()
    {
        RuleFor(p => p.Titulo)
            .NotEmpty()
            .MaximumLength(180);
        
        RuleFor(p => p.Descricao)
            .NotEmpty()
            .MaximumLength(1500);
        
        RuleFor(p => p.Foto)
            .NotEmpty()
            .MaximumLength(1500);
    }
}