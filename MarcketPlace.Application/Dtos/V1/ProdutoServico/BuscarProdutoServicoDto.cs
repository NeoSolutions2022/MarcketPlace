using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Core.Extensions;

namespace MarcketPlace.Application.Dtos.V1.ProdutoServico;

public class BuscarProdutoServicoDto : BuscaPaginadaDto<Domain.Entities.ProdutoServico>
{
    public string? Titulo { get; set; } = null!;
    public string? Descricao { get; set; } = null!; 
    public string? Categoria { get; set; } = null!;
    public double? Preco { get; set; }
    public double? PrecoDesconto { get; set; }

    public override void AplicarFiltro(ref IQueryable<Domain.Entities.ProdutoServico> query)
    {
        var expression = MontarExpressao();

        if (!string.IsNullOrWhiteSpace(Titulo))
        {
            query = query.Where(c => c.Titulo.Contains(Titulo));
        }
        
        if (!string.IsNullOrWhiteSpace(Descricao))
        {
            query = query.Where(c => c.Descricao.Contains(Descricao));
        }

        if (!string.IsNullOrWhiteSpace(Categoria))
        {
            query = query.Where(c => c.Categoria.Contains(Categoria));
        }

        if (Preco != null && Preco.Value != 0)
        {
            query = query.Where(c => c.Preco.Equals(Preco));
        }
        
        if (PrecoDesconto != null && PrecoDesconto.Value != 0)
        {
            query = query.Where(c => c.PrecoDesconto.Equals(PrecoDesconto));
        }

        query = query.Where(expression);
    }

    public override void AplicarOrdenacao(ref IQueryable<Domain.Entities.ProdutoServico> query)
    {
        if (DirecaoOrdenacao.EqualsIgnoreCase("asc"))
        {
            query = OrdenarPor.ToLower() switch
            {
                "titulo" => query.OrderBy(c => c.Titulo),
                "descricao" => query.OrderBy(c => c.Descricao),
                "categoria" => query.OrderBy(c => c.Categoria),
                "preco" => query.OrderBy(c => c.Preco),
                "precoDesconto" => query.OrderBy(c => c.PrecoDesconto),
                "id" or _ => query.OrderBy(c => c.Id)
            };
            return;
        }
        
        query = OrdenarPor.ToLower() switch
        {
            "titulo" => query.OrderByDescending(c => c.Titulo),
            "descricao" => query.OrderByDescending(c => c.Descricao),
            "categoria" => query.OrderByDescending(c => c.Categoria),
            "preco" => query.OrderByDescending(c => c.Preco),
            "precoDesconto" => query.OrderByDescending(c => c.PrecoDesconto),
            "id" or _ => query.OrderByDescending(c => c.Id)
        };
    }
}