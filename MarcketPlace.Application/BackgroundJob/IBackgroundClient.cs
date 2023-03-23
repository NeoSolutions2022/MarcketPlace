using System.Linq.Expressions;
using Hangfire;

namespace MarcketPlace.Application.BackgroundJob;

public interface IBackgroundClient
{
    string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay);
    string Schedule<T>(Expression<Func<T,Task>> methodCall, TimeSpan delay);
}

public class BackgroundClient : IBackgroundClient
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public BackgroundClient(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay)
    {
        return _backgroundJobClient.Schedule(methodCall, delay);
    }

    public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
    {
        return _backgroundJobClient.Schedule(methodCall, delay);
    }
}