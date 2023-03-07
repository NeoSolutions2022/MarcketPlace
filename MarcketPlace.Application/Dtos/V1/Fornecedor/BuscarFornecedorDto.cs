using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Core.Extensions;

namespace MarcketPlace.Application.Dtos.V1.Fornecedor;

public class BuscarFornecedorDto : BuscaPaginadaDto<Domain.Entities.Fornecedor>
{
    public string? Nome { get; set; }
    public string? NomeSocial { get; set; }
    public bool? Desativado { get; set; }

    public override void AplicarFiltro(ref IQueryable<Domain.Entities.Fornecedor> query)
    {
        var expression = MontarExpressao();

        if (!string.IsNullOrWhiteSpace(Nome))
        {
            query = query.Where(c => c.Nome.Contains(Nome));
        }
        
        if (!string.IsNullOrWhiteSpace(NomeSocial))
        {
            query = query.Where(c => c.Nome.Contains(Nome));
        }
        
        if (!string.IsNullOrWhiteSpace(NomeSocial))
        {
            query = query.Where(c => c.NomeSocial!.Contains(NomeSocial));
        }

        if (Desativado.HasValue)
        {
            query = query.Where(c => c.Desativado == Desativado.Value);
        }

        query = query.Where(expression);
    }

    public override void AplicarOrdenacao(ref IQueryable<Domain.Entities.Fornecedor> query)
    {
        if (DirecaoOrdenacao.EqualsIgnoreCase("asc"))
        {
            query = OrdenarPor.ToLower() switch
            {
                "nome" => query.OrderBy(c => c.Nome),
                "nomesocial" => query.OrderBy(c => c.NomeSocial),
                "desativado" => query.OrderBy(c => c.Desativado),
                "id" or _ => query.OrderBy(c => c.Id)
            };
            return;
        }
        
        query = OrdenarPor.ToLower() switch
        {
            "nome" => query.OrderByDescending(c => c.Nome),
            "nomesocial" => query.OrderByDescending(c => c.NomeSocial),
            "desativado" => query.OrderByDescending(c => c.Desativado),
            "id" or _ => query.OrderByDescending(c => c.Id)
        };
    }
}