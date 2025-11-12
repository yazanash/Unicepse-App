using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Exercises;
using Uniceps.API.Services;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services.DataSyncServices;
using Uniceps.Stores.ApiDataStores;

namespace Uniceps.HostBuilders
{
    public static class AddApiServicesHostBuilderExtentions
    {
        public static IHostBuilder AddApiServices(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<PlayerApiDataService>();
                services.AddSingleton<PaymentApiDataService>();
                services.AddSingleton<MetricApiDataService>();
                services.AddSingleton<SubscriptionApiDataService>();
                services.AddSingleton<AttendanceApiDataService>();
                services.AddSingleton<GetExercisesService>();
                services.AddSingleton<SystemAuthApiService>();
                services.AddSingleton<SystemProfileApiDataService>();
                services.AddSingleton<SystemSubscriptionApiDataService>();
                services.AddSingleton<IDataService<SyncObject>, SyncDataService>();
            });
            return _hostBuilder;
        }
    }
}
