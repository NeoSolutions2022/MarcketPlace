using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Authorization;
using MarcketPlace.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace MarcketPlace.Api.Controllers.V1.Cliente;

[Authorize]
[ClaimsAuthorize("Cliente", ETipoUsuario.Cliente)]
public class MainController : BaseController
{
    protected MainController(INotificator notificator) : base(notificator)
    {
    }
}