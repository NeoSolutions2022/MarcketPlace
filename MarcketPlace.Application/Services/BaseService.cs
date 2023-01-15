using AutoMapper;
using MarcketPlace.Application.Notification;

namespace MarcketPlace.Application.Services;

public class BaseService
{
    protected readonly IMapper Mapper;
    protected readonly INotificator Notificator;

    public BaseService(IMapper mapper, INotificator notificator)
    {
        Mapper = mapper;
        Notificator = notificator;
    }
}