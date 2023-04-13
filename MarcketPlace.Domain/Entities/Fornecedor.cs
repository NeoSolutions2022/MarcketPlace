using FluentValidation.Results;
using MarcketPlace.Domain.Contracts;
using MarcketPlace.Domain.Contracts.Repositories;
using MarcketPlace.Domain.Validation;

namespace MarcketPlace.Domain.Entities;

public class Fornecedor : Entity, IAggregateRoot, ISoftDelete, IAnunciavel
{
    public string Cep { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string? Cnpj { get; set; }
    public Guid? CodigoResetarSenha { get; set; }
    public DateTime? CodigoResetarSenhaExpiraEm { get; set; }
    public string? Complemento { get; set; }
    public string Cpf { get; set; } = null!;
    public bool Desativado { get; set; }
    public string Email { get; set; } = null!;
    public string? Descricao { get; set; }
    public string Endereco { get; set; } = null!;
    public string Bairro { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public int Numero { get; set; }
    public string Categoria { get; set; } = null!;
    public string Responsavel { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string? Telefone { get; set; }
    public string Uf { get; set; } = null!;
    public string? Foto { get; set; }
    
    public bool AnuncioPago { get; set; }
    public DateTime? DataPagamentoAnuncio { get; set; }
    public DateTime? DataExpiracaoAnuncio { get; set; }
    public List<ProdutoServico> ProdutoServicos { get; set; } = new();

    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new FornecedorValidator().Validate(this);
        return validationResult.IsValid;
    }

}