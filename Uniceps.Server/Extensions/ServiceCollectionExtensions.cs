using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uniceps.Core.Models.Authentication;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.AuthService;
using Uniceps.Entityframework.Services.PlayerQueries;
using Uniceps.Server.Services;
using Uniceps.Server.SystemServices;

namespace Uniceps.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUniceps(
      this IServiceCollection services,
      IConfiguration configuration)
        {
            bool sqlite = configuration.GetValue<bool>("UseSqlite");

            string connectionString =  configuration.GetConnectionString("default")!;

            services.AddSingleton(new UnicepsDbContextFactory(connectionString, false));

            services.AddSingleton< IPushPullDataService<Player> ,PlayerPushPullDataService>();
            services.AddSingleton<IPushPullDataService<Subscription>, SubscriptionPushPullDataService>();
            services.AddScoped<DatabaseInitialService>();
            return services;
        }
    }
}
