using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Core.Extensions;

namespace MarcketPlace.Application.Dtos.V1.Fornecedor;

public class BuscarFornecedorDto : BuscaPaginadaDto<Domain.Entities.Fornecedor>
{
    public string? Nome { get; set; }
    public string? Responsavel { get; set; }
    public bool? Desativado { get; set; }
    public string? Cep { get; set; }
    public string? Cidade { get; set; }
    public string? Uf { get; set; }

    public override void AplicarFiltro(ref IQueryable<Domain.Entities.Fornecedor> query)
    {
        var expression = MontarExpressao();

        if (!string.IsNullOrWhiteSpace(Nome))
        {
            query = query.Where(c => c.Nome.Contains(Nome));
        }
        
        if (!string.IsNullOrWhiteSpace(Responsavel))
        {
            query = query.Where(c => c.Responsavel.Contains(Responsavel));
        }
        
        if (!string.IsNullOrWhiteSpace(Cep))
        {
            query = query.Where(c => c.Cep.Contains(Cep));
        }
        
        if (!string.IsNullOrWhiteSpace(Uf))
        {
            query = query.Where(c => c.Uf.Contains(Uf));
        }
        
        if (!string.IsNullOrWhiteSpace(Cidade))
        {
            query = query.Where(c => c.Cidade.Contains(Cidade));
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
                "desativado" => query.OrderBy(c => c.Desativado),
                "cep" => query.OrderBy(c => c.Cep),
                "responsavel" => query.OrderBy(c => c.Responsavel),
                "cidade" => query.OrderBy(c => c.Cidade),
                "uf" => query.OrderBy(c => c.Uf),
                "id" or _ => query.OrderBy(c => c.Id)
            };
            return;
        }
        
        query = OrdenarPor.ToLower() switch
        {
            "nome" => query.OrderByDescending(c => c.Nome),
            "cep" => query.OrderByDescending(c => c.Cep),
            "cidade" => query.OrderByDescending(c => c.Cidade),
            "responsavel" => query.OrderByDescending(c => c.Responsavel),
            "uf" => query.OrderByDescending(c => c.Uf),
            "desativado" => query.OrderByDescending(c => c.Desativado),
            "id" or _ => query.OrderByDescending(c => c.Id)
        };
    }
}