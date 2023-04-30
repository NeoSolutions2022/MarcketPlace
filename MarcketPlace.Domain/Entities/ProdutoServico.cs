using FluentValidation.Results;
using MarcketPlace.Domain.Contracts;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Validation;

namespace MarcketPlace.Domain.Entities;

public class ProdutoServico : Entity, IAggregateRoot, ISoftDelete, IAnunciavel
{
    public string? Foto { get; set; }
    public string? Foto2 { get; set; }
    public string? Foto3 { get; set; }
    public string? Foto4 { get; set; }
    public string? Foto5 { get; set; }
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public double Preco { get; set; }
    public double PrecoDesconto { get; set; }
    public bool Desativado { get; set; }
    public int FornecedorId { get; set; }
    public string Categoria { get; set; } = null!;
    public string? Caracteristica { get; set; }
    public Fornecedor Fornecedor { get; set; } = null!;
    public List<ProdutoServicoCaracteristica> ProdutoServicoCaracteristicas { get; set; } = new();

    public bool AnuncioPago { get; set; }
    public DateTime? DataPagamentoAnuncio { get; set; }
    public DateTime? DataExpiracaoAnuncio { get; set; }

    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new ProdutoServicoValidator().Validate(this);
        return validationResult.IsValid;
    }
}