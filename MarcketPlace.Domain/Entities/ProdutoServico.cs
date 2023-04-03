
using FluentValidation.Results;
using MarcketPlace.Domain.Contracts;
using MarcketPlace.Domain.Validation;

namespace MarcketPlace.Domain.Entities;

public class ProdutoServico : Entity, IAggregateRoot, ISoftDelete
{
    public string Foto { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public bool Desativado { get; set; }
    public int FornecedorId { get; set; }
    public Fornecedor Fornecedor { get; set; } = new();
    
    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new ProdutoServicoValidator().Validate(this);
        return validationResult.IsValid;
    }
}