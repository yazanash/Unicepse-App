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
using Uniceps.Entityframework.Services.EmployeeQueries;
using Uniceps.Entityframework.Services.EmployeeTransaction;
using Uniceps.Entityframework.Services.GetPlayerTransaction;
using Uniceps.Entityframework.Services.PlayerQueries;
using Uniceps.Entityframework.Services.PublicIdServices;
using Uniceps.Entityframework.Services.RelationService;
using Uniceps.Entityframework.Services.RoutineService;
using Uniceps.Entityframework.Services.SportTransactions;
using Uniceps.Entityframework.Services.TransactionsReportServices;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.Entityframework.Services.DataSyncServices;

namespace Uniceps.HostBuilders
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



                services.AddSingleton<IGetExercisesService, RoutineExercisesService>();


                services.AddSingleton<IDeleteConnectionService<Sport>, SportTrainersDeleteConnection>();
                services.AddSingleton<IDeleteConnectionService<Employee>, TrainerSportsDeleteConnection>();
                services.AddSingleton<IEmployeeQuery, PercentEmployeeQuery>();




                services.AddSingleton<IArchivedService<Player>, ArchivedPlayerService>();


                services.AddSingleton<IPublicIdService<Player>, PlayerUidService>();


            });
            return _hostBuilder;
        }
    }
}
