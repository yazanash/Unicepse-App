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

namespace Uniceps.HostBuilders
{
    public static class AddServicesHostBuilderExtentions
    {
        public static IHostBuilder AddServices(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
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
                services.AddSingleton<IDataService<License>, LicenseDataService>();
                services.AddSingleton<IDataService<GymProfile>, GymProfileDataService>();
                services.AddSingleton<PlayersAttendenceService>();
                services.AddSingleton<ExpensesDataService>();


                services.AddSingleton<IDataService<RoutineModel>, RoutineModelDataService>();
                services.AddSingleton<IDataService<DayGroup>, RoutineDayGroupDataService>();
                services.AddSingleton<IDataService<RoutineItemModel>, RoutineItemDataService>();
                services.AddSingleton<IGetAllById<DayGroup>, RoutineDayGroupDataService>();
                services.AddSingleton<IGetAllById<RoutineItemModel>, RoutineItemDataService>();

                services.AddSingleton<IDataService<SetModel>, RoutineSetsDataService>();
                services.AddSingleton<IGetAllById<SetModel>, RoutineSetsDataService>();
            });
            return _hostBuilder;
        }
    }
}
