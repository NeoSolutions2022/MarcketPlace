
using MarcketPlace.Domain.Contracts;

namespace MarcketPlace.Domain.Entities;

public class ProdutoServico : Entity, IAggregateRoot, ISoftDelete
{
    public string Foto { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public bool Desativado { get; set; }
    public int FornecedorId { get; set; }
    public Fornecedor Fornecedor { get; set; }
}