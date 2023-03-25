using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Core.Extensions;

namespace MarcketPlace.Application.Dtos.V1.Cliente;

public class BuscarClienteDto : BuscaPaginadaDto<Domain.Entities.Cliente>
{
    public string? Nome { get; set; }
    public string? NomeSocial { get; set; }
    public bool? Inadiplente { get; set; }
    public DateTime? DataPagamento { get; set; }
    public bool? Desativado { get; set; }
    public string? Cep { get; set; }
    public string? Cidade { get; set; }
    public string? Uf { get; set; }

    public override void AplicarFiltro(ref IQueryable<Domain.Entities.Cliente> query)
    {
        var expression = MontarExpressao();

        if (!string.IsNullOrWhiteSpace(Nome))
        {
            query = query.Where(c => c.Nome.Contains(Nome));
        }
        
        if (!string.IsNullOrWhiteSpace(NomeSocial))
        {
            query = query.Where(c => c.NomeSocial!.Contains(NomeSocial));
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
        
        if (Inadiplente.HasValue)
        {
            query = query.Where(c => c.Inadiplente == Inadiplente.Value);
        }
        
        if (DataPagamento.HasValue)
        {
            query = query.Where(c => c.DataPagamento == DataPagamento.Value);
        }
        
        if (Desativado.HasValue)
        {
            query = query.Where(c => c.Desativado == Desativado.Value);
        }

        query = query.Where(expression);
    }

    public override void AplicarOrdenacao(ref IQueryable<Domain.Entities.Cliente> query)
    {
        if (DirecaoOrdenacao.EqualsIgnoreCase("asc"))
        {
            query = OrdenarPor.ToLower() switch
            {
                "nome" => query.OrderBy(c => c.Nome),
                "nomesocial" => query.OrderBy(c => c.NomeSocial),
                "inadiplente" => query.OrderBy(c => c.Inadiplente),
                "datapagamento" => query.OrderBy(c => c.DataPagamento),
                "cep" => query.OrderBy(c => c.Cep),
                "cidade" => query.OrderBy(c => c.Cidade),
                "uf" => query.OrderBy(c => c.Uf),
                "desativado" => query.OrderBy(c => c.Desativado),
                "id" or _ => query.OrderBy(c => c.Id)
            };
            return;
        }
        
        query = OrdenarPor.ToLower() switch
        {
            "nome" => query.OrderByDescending(c => c.Nome),
            "nomesocial" => query.OrderByDescending(c => c.NomeSocial),
            "inadiplente" => query.OrderByDescending(c => c.Inadiplente),
            "cep" => query.OrderByDescending(c => c.Cep),
            "cidade" => query.OrderByDescending(c => c.Cidade),
            "uf" => query.OrderByDescending(c => c.Uf),
            "datapagamento" => query.OrderByDescending(c => c.DataPagamento),
            "desativado" => query.OrderByDescending(c => c.Desativado),
            "id" or _ => query.OrderByDescending(c => c.Id)
        };
    }
}