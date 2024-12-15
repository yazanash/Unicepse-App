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
using Unicepse.Entityframework.Services.DataSyncServices;
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
                services.AddSingleton<IDataService<Player>, PlayerDataService>();

                services.AddSingleton<IDataService<Sport>, SportServices>();
                services.AddSingleton<IDataService<Employee>, EmployeeDataService>();
                services.AddSingleton<IDataService<Metric>, MetricDataService>();
                services.AddSingleton<IDataService<PlayerRoutine>, PlayerRoutineDataService>();
                services.AddSingleton<IDataService<Credit>, EmployeeCreditsDataService>();
                services.AddSingleton<IDataService<PlayerPayment>, PaymentDataService>();
                services.AddSingleton<IDataService<Subscription>, SubscriptionDataService>();
                services.AddSingleton<IDataService<License>, LicenseDataService>();
                services.AddSingleton<IDataService<GymProfile>, GymProfileDataService>();
                services.AddSingleton<PlayersAttendenceService>();
                services.AddSingleton<ExpensesDataService>();
               
              
            });
            return _hostBuilder;
        }
    }
}
