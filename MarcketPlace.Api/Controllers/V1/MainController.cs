using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Authorization;
using MarcketPlace.Core.Enums;

namespace MarcketPlace.Api.Controllers.V1;

[ClaimsAuthorize("TipoUsuario", ETipoUsuario.Comum)]
public class MainController : BaseController
{
    protected MainController(INotificator notificator) : base(notificator)
    {
    }
}