using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Stores;
using Uniceps.Stores.EmployeeStores;
using Uniceps.Stores.RoutineStores;
using Uniceps.Stores.SportStores;
using Uniceps.Core.Models.Authentication;
using Uniceps.navigation.Stores;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.Helpers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services;
using Uniceps.DataExporter;

namespace Uniceps.HostBuilders
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
                services.AddSingleton<AuthenticationStore>();
                services.AddSingleton<EmployeeSubscriptionDataStore>();
                services.AddSingleton<SportSubscriptionDataStore>();
                services.AddSingleton<DailyReportStore>();
                services.AddSingleton<ExercisesDataStore>();
                services.AddSingleton<PeriodReportStore>();

                services.AddSingleton<DayGroupDataStore>();
                services.AddSingleton<RoutineItemDataStore>();
                services.AddSingleton<RoutineTempDataStore>();
                services.AddSingleton<SetsModelDataStore>();

                services.AddSingleton<ISystemAuthStore,SystemAuthStore>();
                services.AddSingleton<SessionValidator>();
                services.AddSingleton<UserContextValidator>();
                services.AddSingleton<UserFlowService>();
                services.AddSingleton<IProfileDataStore, SystemProfileStore>();
                services.AddSingleton<SystemSubscriptionStore>();
                services.AddSingleton<DataExportStore>();
            });
            return _hostBuilder;
        }
    }
}
