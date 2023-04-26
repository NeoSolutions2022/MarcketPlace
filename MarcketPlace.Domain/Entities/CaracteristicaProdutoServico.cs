using FluentValidation.Results;

namespace MarcketPlace.Domain.Entities;

public class CaracteristicaProdutoServico : EntityNotTracked
{
    public string? Chave { get; set; }
    public string? Valor { get; set; }
    public int ProdutoServicoId { get; set; }

    public ProdutoServico ProdutoServico { get; set; } = null!;

    // public override bool Validar(out ValidationResult validationResult)
    // {
    //     validationResult = new CaracteristicaProdutoServicoValidator().Validate(this);
    //     return validationResult.IsValid;
    // }
}