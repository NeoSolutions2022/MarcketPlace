using MarcketPlace.Application.Dtos.V1.Base;
using MarcketPlace.Core.Extensions;

namespace MarcketPlace.Application.Dtos.V1.Administrador;

public class BuscarAdministradorDto : BuscaPaginadaDto<Domain.Entities.Administrador>
{
    public string? Nome { get; set; }
    public string? Email { get; set; }

    public override void AplicarFiltro(ref IQueryable<Domain.Entities.Administrador> query)
    {
        var expression = MontarExpressao();

        if (!string.IsNullOrWhiteSpace(Nome))
        {
            query = query.Where(c => c.Nome.Contains(Nome));
        }
        
        if (!string.IsNullOrWhiteSpace(Email))
        {
            query = query.Where(c => c.Nome.Contains(Email));
        }

        query = query.Where(expression);
    }

    public override void AplicarOrdenacao(ref IQueryable<Domain.Entities.Administrador> query)
    {
        if (DirecaoOrdenacao.EqualsIgnoreCase("asc"))
        {
            query = OrdenarPor.ToLower() switch
            {
                "nome" => query.OrderBy(c => c.Nome),
                "email" => query.OrderBy(c => c.Email),
                "id" or _ => query.OrderBy(c => c.Id)
            };
            return;
        }
        
        query = OrdenarPor.ToLower() switch
        {
            "nome" => query.OrderByDescending(c => c.Nome),
            "email" => query.OrderByDescending(c => c.Email),
            "id" or _ => query.OrderByDescending(c => c.Id)
        };
    }
}