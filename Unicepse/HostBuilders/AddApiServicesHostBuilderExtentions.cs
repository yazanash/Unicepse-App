using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Services;
using Unicepse.Core.Models.SyncModel;
using Unicepse.Core.Services;
using Unicepse.Entityframework.Services.DataSyncServices;
using Unicepse.Stores.ApiDataStores;

namespace Unicepse.HostBuilders
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
                services.AddSingleton<RoutineApiDataService>();
                services.AddSingleton<AttendanceApiDataService>();
                services.AddSingleton<LicenseApiDataService>();
                services.AddSingleton<IDataService<SyncObject>,SyncDataService>();
            });
            return _hostBuilder;
        }
    }
}
