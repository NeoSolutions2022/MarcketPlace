
using FluentValidation.Results;
using MarcketPlace.Domain.Contracts;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Validation;

namespace MarcketPlace.Domain.Entities;

public class ProdutoServico : Entity, IAggregateRoot, ISoftDelete, IAnunciavel
{
    public string Foto { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public bool Desativado { get; set; }
    public int FornecedorId { get; set; }
    public string Categoria { get; set; } = null!;
    public Fornecedor Fornecedor { get; set; } = new();
    
    public bool AnuncioPago { get; set; }
    public DateTime? DataPagamentoAnuncio { get; set; }
    public DateTime? DataExpiracaoAnuncio { get; set; }
    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new ProdutoServicoValidator().Validate(this);
        return validationResult.IsValid;
    }

}