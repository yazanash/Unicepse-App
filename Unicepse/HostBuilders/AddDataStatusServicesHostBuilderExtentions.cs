using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models;
using Unicepse.Core.Models.DailyActivity;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Expenses;
using Unicepse.Core.Models.Metric;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Core.Services;
using Unicepse.Entityframework.Services.DataSyncServices;
using Unicepse.Entityframework.Services.EmployeeQueries;
using Unicepse.Entityframework.Services.EmployeeTransaction;
using Unicepse.Entityframework.Services.GetPlayerTransaction;
using Unicepse.Entityframework.Services.LicenseQuery;
using Unicepse.Entityframework.Services.PlayerQueries;
using Unicepse.Entityframework.Services.PublicIdServices;
using Unicepse.Entityframework.Services.RelationService;
using Unicepse.Entityframework.Services.RoutineService;
using Unicepse.Entityframework.Services.SportTransactions;
using Unicepse.Entityframework.Services.TransactionsReportServices;

namespace Unicepse.HostBuilders
{
    public static class AddDataStatusServicesHostBuilderExtentions
    {
        public static IHostBuilder AddDataStatusServices(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IGetPlayerTransactionService<PlayerPayment>, GetPaymentService>();
                services.AddSingleton<IGetPlayerTransactionService<Subscription>, GetSubscriptionsService>();
                services.AddSingleton<IGetPlayerTransactionService<Metric>, GetMetricsService>();
                services.AddSingleton<IGetPlayerTransactionService<PlayerRoutine>, GetRoutineService>();

                services.AddSingleton<IDailyTransactionService<Expenses>, ExpensesDailyReportService>();
                services.AddSingleton<IDailyTransactionService<Subscription>, SubscriptionsDailyReportService>();
                services.AddSingleton<IDailyTransactionService<PlayerPayment>, PaymentDailyReportService>();
                services.AddSingleton<IDailyTransactionService<Credit>, CreditsDailyReportService>();

                services.AddSingleton<IPeriodReportService<PlayerPayment>, PaymentsPeriodReportService>();
                services.AddSingleton<IPeriodReportService<Expenses>, ExpensesPeriodReportService>();

                services.AddSingleton<IEmployeeMonthlyTransaction<PlayerPayment>, PaymentEmployeeMonthlyService>();
                services.AddSingleton<IEmployeeMonthlyTransaction<Credit>, CreditEmployeeMonthlyService>();
                services.AddSingleton<IEmployeeMonthlyTransaction<Subscription>, SubscriptionEmployeeMonthlyService>();

                services.AddSingleton<IEmployeeTransaction<Credit>, CreditEmployeeService>();
                services.AddSingleton<IEmployeeTransaction<Subscription>, SubscriptionEmployeeService>();

                services.AddSingleton<ISportMonthlyTransactions<Subscription>, SubscriptionSportMonthlyReportService>();
                services.AddSingleton<IActiveTransactionService<Subscription>, ActiveSubsecriptionService>();


                services.AddSingleton<IRoutineItemsDataService, RoutineItemsService>();
                services.AddSingleton<IGetExercisesService, RoutineExercisesService>();
                services.AddSingleton<IGetRoutineTemplatesService, RoutineTemplateService>();


                services.AddSingleton<IDeleteConnectionService<Sport>, SportTrainersDeleteConnection>();
                services.AddSingleton<IDeleteConnectionService<Employee>, TrainerSportsDeleteConnection>();
                services.AddSingleton<IEmployeeQuery, PercentEmployeeQuery>();




                services.AddSingleton<IArchivedService<Player>, ArchivedPlayerService>();
                services.AddSingleton<IGetSingleLatestService<License>, LicenseGetLatestService>();
                services.AddSingleton<IGetSingleLatestService<GymProfile>, GymProfileGetLatestService>();


                services.AddSingleton<IPublicIdService<Player>, PlayerUidService>();
                services.AddSingleton<IPublicIdService<License>, LicensePidService>();
                services.AddSingleton<IPublicIdService<GymProfile>, GymPidService>();


            });
            return _hostBuilder;
        }
    }
}
