using FluentValidation;
using MarcketPlace.Domain.Entities;

namespace MarcketPlace.Domain.Validation;

public class ProdutoServicoValidator : AbstractValidator<ProdutoServico>
{
    public ProdutoServicoValidator()
    {
        RuleFor(p => p.Titulo)
            .NotEmpty()
            .WithMessage("Titulo não pode ser vazio")
            .MaximumLength(180)
            .WithMessage("Titulo deve ter no máximo 180 caracteres");

        RuleFor(p => p.Descricao)
            .NotEmpty()
            .WithMessage("Descricao não pode ser vazio")
            .MaximumLength(1500)
            .WithMessage("Descricao deve ter no máximo 1500 caracteres");

        RuleFor(p => p.Foto)
            .MaximumLength(1500)
            .WithMessage("Foto deve ter no máximo 1500 caracteres");

        RuleFor(c => c.Preco)
            .NotNull()
            .WithMessage("Preco não pode ser nulo")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Preço deve ser maior que 0");
        
        RuleFor(c => c.PrecoDesconto)
            .NotNull()
            .WithMessage("PrecoDesconto não pode ser nulo")
            .GreaterThanOrEqualTo(0)
            .WithMessage("PrecoDesconto deve ser maior que 0");

        RuleFor(c => c.Categoria)
            .NotNull()
            .WithMessage("Categoria não pode ser nula")
            .NotEmpty()
            .WithMessage("Categoria deve conter ao menos um item");
    }
}