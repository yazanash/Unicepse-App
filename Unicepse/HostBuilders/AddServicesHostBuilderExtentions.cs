using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Services;
using Unicepse.Core.Models.Authentication;
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
                services.AddSingleton<AuthenticationService>();
                services.AddSingleton<PlayerDataService>();
                services.AddSingleton<SportServices>();
                services.AddSingleton<EmployeeDataService>();
                services.AddSingleton<DausesDataService>();
                services.AddSingleton<EmployeeCreditsDataService>();
                services.AddSingleton<PaymentDataService>();
                services.AddSingleton<PlayersAttendenceService>();
                services.AddSingleton<SubscriptionDataService>();
                services.AddSingleton<ExpensesDataService>();
                services.AddSingleton<MetricDataService>();
                services.AddSingleton<PlayerRoutineDataService>();
                services.AddSingleton<PlayerApiDataService>();
                services.AddSingleton<PaymentApiDataService>();
                services.AddSingleton<MetricApiDataService>();
                services.AddSingleton<SubscriptionApiDataService>();
                services.AddSingleton<RoutineApiDataService>();
                services.AddSingleton<AttendanceApiDataService>();
                services.AddSingleton<LicenseApiDataService>();
                services.AddSingleton<LicenseDataService>();
                services.AddSingleton<GymProfileDataService>();

            });
            return _hostBuilder;
        }
    }
}
