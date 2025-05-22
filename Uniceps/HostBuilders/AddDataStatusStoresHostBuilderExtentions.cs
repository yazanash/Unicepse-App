using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.Stores.ApiDataStores;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.Stores;

namespace Uniceps.HostBuilders
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
                services.AddSingleton<IApiDataStore<PlayerPayment>, PaymentsApiDataStore>();
                services.AddSingleton<IDeleteApiDataStore<PlayerPayment>, PaymentsApiDataStore>();
                services.AddSingleton<IApiDataStore<DailyPlayerReport>, AttendanceApiDataStore>();
                services.AddSingleton<SyncStore>();
            });
            return _hostBuilder;
        }
    }
}
