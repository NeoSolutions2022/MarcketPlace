using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Authorization;
using MarcketPlace.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace MarcketPlace.Api.Controllers.V1.Gerencia;

[Authorize]
[ClaimsAuthorize("TipoUsuario", ETipoUsuario.Administrador)]
public abstract class MainController : BaseController
{
    protected MainController(INotificator notificator) : base(notificator)
    {
    }
}