using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services.PlayerQueries;
using Uniceps.Entityframework.Services.RelationService;
using Uniceps.Entityframework.Services.RoutineService;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.Entityframework.Services.DataSyncServices;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.DataExportProvider;

namespace Uniceps.HostBuilders
{
    public static class AddDataStatusServicesHostBuilderExtentions
    {
        public static IHostBuilder AddDataStatusServices(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IDailyReportService, DailyReportService>();
                services.AddSingleton<IPeriodReportService, PeriodReportService>();
                services.AddSingleton<ITrainerRevenueService, TrainerRevenueService>();
                services.AddSingleton<IMonthlyReportService, MonthlyReportService>();
                services.AddSingleton<IGetPlayerTransactionService<PlayerPayment>, PaymentDataService>();
                services.AddSingleton<IGetPlayerTransactionService<Subscription>, SubscriptionDataService>();
                services.AddSingleton<IGetPlayerTransactionService<Metric>, MetricDataService>();
                services.AddSingleton<IEmployeeMonthlyTransaction<Credit>, EmployeeCreditsDataService>();
                services.AddSingleton<IEmployeeMonthlyTransaction<Subscription>, SubscriptionDataService>();
                services.AddSingleton<IEmployeeTransaction<Credit>, EmployeeCreditsDataService>();
                services.AddSingleton<IEmployeeTransaction<Subscription>, SubscriptionDataService>();
                services.AddSingleton<ISportMonthlyTransactions<Subscription>, SubscriptionDataService>();
                services.AddSingleton<IGetExercisesService, RoutineExercisesService>();
                services.AddSingleton<IDeleteConnectionService<Sport>, SportTrainersDeleteConnection>();
                services.AddSingleton<IDeleteConnectionService<Employee>, TrainerSportsDeleteConnection>();
                services.AddSingleton<IArchivedService<Player>, PlayerDataService>();
                services.AddSingleton<IPublicIdService<Player>, PlayerDataService>();
                services.AddSingleton<IExportDataProvider, ExportDataProviderService>();
                services.AddSingleton<IImportDataProvider, ImportDataProviderService>();

            });
            return _hostBuilder;
        }
    }
}
