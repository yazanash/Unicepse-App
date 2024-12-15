using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.DailyActivity;
using Unicepse.Core.Models.Metric;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Models.SyncModel;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Stores;
using Unicepse.Stores.ApiDataStores;

namespace Unicepse.HostBuilders
{
    public static class AddDataStatusStoresHostBuilderExtentions
    {
        public static IHostBuilder AddDataStatusStores(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {

                services.AddSingleton<IApiDataStore<Player>, PlayerApiDataStore>();
                services.AddSingleton<IApiDataStore<Subscription>, SubscriptionApiDataStore>();
                services.AddSingleton<IApiDataStore<Metric>, MetricApiDataStore>();
                services.AddSingleton<IApiDataStore<PlayerRoutine>, RoutineApiDataStore>();
                services.AddSingleton<IApiDataStore<PlayerPayment>, PaymentsApiDataStore>();
                services.AddSingleton<IDeleteApiDataStore<PlayerPayment>, PaymentsApiDataStore>(); 
                services.AddSingleton<IApiDataStore<DailyPlayerReport>, AttendanceApiDataStore>();
                services.AddSingleton<SyncStore>();
            });
            return _hostBuilder;
        }
    }
}
