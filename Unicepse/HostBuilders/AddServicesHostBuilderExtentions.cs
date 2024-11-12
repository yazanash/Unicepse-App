using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.API.Services;
using Unicepse.Commands.Player;
using Unicepse.Core.Models;
using Unicepse.Core.Models.Authentication;
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
using Unicepse.Entityframework.Services;
using Unicepse.Entityframework.Services.AuthService;
using Unicepse.Entityframework.Services.PlayerQueries;

namespace Unicepse.HostBuilders
{
    public static class AddServicesHostBuilderExtentions
    {
        public static IHostBuilder AddServices(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IPasswordHasher, PasswordHasher>();
                services.AddSingleton<IAccountDataService<User>, AccountDataService>();
                services.AddSingleton<IAuthenticationService, AuthenticationService>();
                services.AddSingleton<IPlayerDataService, PlayerDataService>();
                services.AddSingleton<ISportDataService, SportServices>();
                services.AddSingleton<IEmployeeDataStore, EmployeeDataService>();
                services.AddSingleton<DausesDataService>();
                services.AddSingleton<ICreditDataService, EmployeeCreditsDataService>();
                services.AddSingleton<IPaymentDataService, PaymentDataService>();
                services.AddSingleton<PlayersAttendenceService>();
                services.AddSingleton<ISubscriptionDataService, SubscriptionDataService>();
                services.AddSingleton<ExpensesDataService>();
                services.AddSingleton<IMetricDataService, MetricDataService>();
                services.AddSingleton<IRoutineDateService, PlayerRoutineDataService>();

                services.AddSingleton<PlayerApiDataService>();
                services.AddSingleton<PaymentApiDataService>();
                services.AddSingleton<MetricApiDataService>();
                services.AddSingleton<SubscriptionApiDataService>();
                services.AddSingleton<RoutineApiDataService>();
                services.AddSingleton<AttendanceApiDataService>();
                services.AddSingleton<LicenseApiDataService>();

                services.AddSingleton<ILicenseDataService, LicenseDataService>();
                services.AddSingleton<IGymProfileDataService, GymProfileDataService>();
                /////////////////////////////
                ///commands
                ////////////////////////////
                services.AddSingleton<UpdateCurrentViewModelCommand>();


            });
            return _hostBuilder;
        }
    }
}
