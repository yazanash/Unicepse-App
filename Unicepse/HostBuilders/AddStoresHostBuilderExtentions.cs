using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.navigation.Stores;
using Unicepse.Stores;

namespace Unicepse.HostBuilders
{
    public static class AddStoresHostBuilderExtentions
    {
        public static IHostBuilder AddStores(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton(new NavigationStore());
                services.AddSingleton<AccountStore>();
                services.AddSingleton<UsersDataStore>();
                services.AddSingleton<PlayersDataStore>();
                services.AddSingleton<SportDataStore>();
                services.AddSingleton<EmployeeStore>();
                services.AddSingleton<BackgroundServiceStore>();
                services.AddSingleton<GymStore>();
                services.AddSingleton<CreditsDataStore>();
                services.AddSingleton<DausesDataStore>();
                services.AddSingleton<PaymentDataStore>();
                services.AddSingleton<PlayersAttendenceStore>();
                services.AddSingleton<SubscriptionDataStore>();
                services.AddSingleton<ExpensesDataStore>();
                services.AddSingleton<MetricDataStore>();
                services.AddSingleton<RoutineDataStore>();
                services.AddSingleton<LicenseDataStore>();
                services.AddSingleton<AuthenticationStore>();
            });
            return _hostBuilder;
        }
    }
}
