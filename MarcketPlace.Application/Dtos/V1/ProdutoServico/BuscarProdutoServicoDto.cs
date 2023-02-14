using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Core.Extensions;

namespace MarcketPlace.Application.Dtos.V1.ProdutoServico;

public class BuscarProdutoServicoDto : BuscaPaginadaDto<Domain.Entities.ProdutoServico>
{
    public string? Titulo { get; set; } = null!;
    public string? Descricao { get; set; } = null!;

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
                "id" or _ => query.OrderBy(c => c.Id)
            };
            return;
        }
        
        query = OrdenarPor.ToLower() switch
        {
            "titulo" => query.OrderByDescending(c => c.Titulo),
            "descricao" => query.OrderByDescending(c => c.Descricao),
            "id" or _ => query.OrderByDescending(c => c.Id)
        };
    }
}