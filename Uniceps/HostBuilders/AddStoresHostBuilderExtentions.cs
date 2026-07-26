using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Uniceps.Stores;
using Uniceps.Stores.RoutineStores;
using Uniceps.navigation.Stores;
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
                services.AddSingleton<DailyReportStore>();
                services.AddSingleton<ExercisesDataStore>();
                services.AddSingleton<PeriodReportStore>();

                services.AddSingleton<DayGroupDataStore>();
                services.AddSingleton<RoutineItemDataStore>();
                services.AddSingleton<RoutineTempDataStore>();
                services.AddSingleton<SetsModelDataStore>();

                services.AddSingleton<DataExportStore>();
                services.AddSingleton<LicenseStore>();
                services.AddSingleton<OtherRevenuesDataStore>();
            });
            return _hostBuilder;
        }
    }
}
