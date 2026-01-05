using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Authentication;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.AuthService;
using Uniceps.Entityframework.Services.PlayerQueries;
using Uniceps.Entityframework.Services.RoutineSystemServices;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.utlis.common;
using Uniceps.Helpers;
using Uniceps.Services;
using Uniceps.DataExporter;

namespace Uniceps.HostBuilders
{
    public static class AddServicesHostBuilderExtentions
    {
        public static IHostBuilder AddServices(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<ISessionManager, SessionManager>();
                services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
                services.AddSingleton<IAccountDataService<User>, AccountDataService>();
                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<IDataService<Player>, PlayerDataService>();

                services.AddSingleton<IDataService<Sport>, SportServices>();
                services.AddSingleton<IDataService<Employee>, EmployeeDataService>();
                services.AddSingleton<IDataService<Metric>, MetricDataService>();
                services.AddSingleton<IDataService<Credit>, EmployeeCreditsDataService>();
                services.AddSingleton<IDataService<PlayerPayment>, PaymentDataService>();
                services.AddSingleton<IDataService<Subscription>, SubscriptionDataService>();
                services.AddSingleton<ISubscriptionRenewService, SubscriptionDataService>();

                services.AddSingleton<PlayersAttendenceService>();
                services.AddSingleton<ExpensesDataService>();


                services.AddSingleton<IDataService<RoutineModel>, RoutineModelDataService>();
                services.AddSingleton<IDataService<DayGroup>, RoutineDayGroupDataService>();
                services.AddSingleton<IUpdateRangeDataService<DayGroup>, RoutineDayGroupDataService>();

                services.AddSingleton<IDataService<RoutineItemModel>, RoutineItemDataService>();
                services.AddSingleton<IUpdateRangeDataService<RoutineItemModel>, RoutineItemDataService>();
                services.AddSingleton<IGetAllById<DayGroup>, RoutineDayGroupDataService>();
                services.AddSingleton<IGetAllById<RoutineItemModel>, RoutineItemDataService>();

                services.AddSingleton<IDataService<SetModel>, RoutineSetsDataService>();
                services.AddSingleton<IGetAllById<SetModel>, RoutineSetsDataService>();
                services.AddSingleton<IUpdateRangeDataService<SetModel>, RoutineSetsDataService>();
                services.AddSingleton<IApplySetsToAll, RoutineSetsDataService>();

                services.AddSingleton<IProfileDataService, SystemProfileDataService>();
                services.AddSingleton<ISystemSubscriptionDataService, SystemSubscriptionDataService>();
                services.AddSingleton<IExcelService<Player>, PlayersExcelService>();
                services.AddSingleton<DataExportManager>();
                services.AddSingleton<ImportManager>();
                services.AddSingleton<TrainerDuesExcelService>();
            });
            return _hostBuilder;
        }
    }
}
