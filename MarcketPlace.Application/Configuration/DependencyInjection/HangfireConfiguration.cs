using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarcketPlace.Application.Configuration.DependencyInjection;

public static class HangfireConfiguration
{
    public static void AddHangfireConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HangfireConnection");

        var mySqlStorage = new SqlServerStorage(connectionString, new SqlServerStorageOptions()
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true // Migration to Schema 7 is required
        });

        services.AddHangfire(config =>
        {
            config
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();

            config
                .UseStorage(mySqlStorage);
        });

        JobStorage.Current = mySqlStorage;
    }
}