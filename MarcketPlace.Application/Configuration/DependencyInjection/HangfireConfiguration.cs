using System.Transactions;
using Hangfire;
using Hangfire.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarcketPlace.Application.Configuration.DependencyInjection;

public static class HangfireConfiguration
{
    public static void AddHangfireConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HangfireConnection");

        var mySqlStorage = new MySqlStorage(connectionString, new MySqlStorageOptions
        {
            TransactionIsolationLevel = IsolationLevel.ReadCommitted,
            QueuePollInterval = TimeSpan.FromSeconds(30),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true,
            DashboardJobListLimit = 50000,
            TransactionTimeout = TimeSpan.FromMinutes(1),
            TablesPrefix = null
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