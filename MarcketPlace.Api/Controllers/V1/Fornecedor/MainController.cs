using MarcketPlace.Application.Notification;
using Microsoft.AspNetCore.Authorization;

namespace MarcketPlace.Api.Controllers.V1.Fornecedor;

[Authorize]
public class MainController : BaseController
{
    protected MainController(INotificator notificator) : base(notificator)
    {
    }
}