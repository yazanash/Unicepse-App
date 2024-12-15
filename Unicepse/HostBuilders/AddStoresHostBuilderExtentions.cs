using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Authentication;
using Unicepse.navigation.Stores;
using Unicepse.Stores;
using Unicepse.Stores.AccountantStores;
using Unicepse.Stores.EmployeeStores;
using Unicepse.Stores.RoutineStores;
using Unicepse.Stores.SportStores;

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
                services.AddSingleton<AccountantDailyStore>();
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
                services.AddSingleton<EmployeeSubscriptionDataStore>();

                services.AddSingleton<PaymentsDailyAccountantStore>();
                services.AddSingleton<PaymentAccountantDataStore>();

                services.AddSingleton<CreditsAccountantDataStore>();
                services.AddSingleton<CreditsDailyAccountantStore>();

                services.AddSingleton<DausesAccountantDataStore>();

                services.AddSingleton<ExpansesDailyAccountantDataStore>();
                services.AddSingleton<ExpensesAccountantDataStore>();

                services.AddSingleton<SubscriptionDailyAccountantDataStore>();

                services.AddSingleton<SportSubscriptionDataStore>();

                services.AddSingleton<ExercisesDataStore>();
                services.AddSingleton<RoutineTemplatesDataStore>();
            });
            return _hostBuilder;
        }
    }
}
