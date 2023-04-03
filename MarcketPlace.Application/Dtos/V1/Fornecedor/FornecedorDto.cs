using MarcketPlace.Application.Dtos.V1.ProdutoServico;

namespace MarcketPlace.Application.Dtos.V1.Fornecedor;

public class FornecedorDto
{
    public int Id { get; set; }
    public string Cep { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string? Cnpj { get; set; }
    public string? Complemento { get; set; }
    public string Cpf { get; set; } = null!;
    public bool Desativado { get; set; }
    public string Email { get; set; } = null!;
    public string Endereco { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public int Numero { get; set; }
    public string Responsavel { get; set; } = null!;
    public string? Telefone { get; set; }
    public string Uf { get; set; } = null!;
    public string? Foto { get; set; }
    public List<ProdutoServicoDto> ProdutoServicos { get; set; }
}