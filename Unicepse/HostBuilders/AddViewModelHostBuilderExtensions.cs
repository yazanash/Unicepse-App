using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Unicepse.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;
using Unicepse.ViewModels.Authentication;
using Unicepse.ViewModels._ِAppViewModels;
using Unicepse.Views.AuthView;
using Unicepse.ViewModels.Accountant;
using Unicepse.navigation.Navigator;
using Unicepse.ViewModels.SportsViewModels;
using Unicepse.ViewModels.Employee.TrainersViewModels;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.navigation.Stores;
using Unicepse.navigation;
using Microsoft.Extensions.Logging;

namespace Unicepse.HostBuilders
{
    public static class AddViewModelHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<AuthViewModel>();
                services.AddSingleton<LicenseViewModel>();
                services.AddTransient((s) => CreateHomeListingViewModel(s));
                services.AddTransient((s) => CreatePlayerListingViewModel(s));
                services.AddTransient((s) => CreateSportListingViewModel(s));
                services.AddTransient((s) => CreateEmployeeListingViewModel(s));
                services.AddTransient((s) => CreateUserListingViewModel(s));
                services.AddSingleton<AccountingViewModel>();
                services.AddTransient<Func<PlayerListViewModel>>(services => () => services.GetRequiredService<PlayerListViewModel>());
                services.AddTransient<NavigationService<PlayerListViewModel>>();
                services.AddSingleton<INavigator, Navigator>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>(),
                });
                services.AddSingleton(s => new AuthWindow()
                {
                    DataContext = s.GetRequiredService<AuthViewModel>(),
                });
                services.AddSingleton(s => new LicenseWindow()
                {
                    DataContext = s.GetRequiredService<LicenseViewModel>(),
                });
            });
            return _hostBuilder;
        }
        private static PlayerListViewModel CreatePlayerListingViewModel(IServiceProvider services)
        {
            return PlayerListViewModel.LoadViewModel(
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<PlayersDataStore>(),
                services.GetRequiredService<SubscriptionDataStore>(),
                services.GetRequiredService<SportDataStore>(),
                services.GetRequiredService<PaymentDataStore>(),
                services.GetRequiredService<MetricDataStore>(),
                services.GetRequiredService<RoutineDataStore>(),
                services.GetRequiredService<PlayersAttendenceStore>(),
                services.GetRequiredService<LicenseDataStore>(),
                services.GetRequiredService<NavigationService<PlayerListViewModel>>(),
                services.GetRequiredService<ILogger<PlayerListViewModel>>()
                );
        }
        private static SportListViewModel CreateSportListingViewModel(IServiceProvider services)
        {
            return SportListViewModel.LoadViewModel(
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<SportDataStore>(),
                services.GetRequiredService<EmployeeStore>(),
                services.GetRequiredService<SubscriptionDataStore>());
        }
        private static TrainersListViewModel CreateEmployeeListingViewModel(IServiceProvider services)
        {
            return TrainersListViewModel.LoadViewModel(
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<EmployeeStore>(),
                services.GetRequiredService<SportDataStore>(),
                services.GetRequiredService<SubscriptionDataStore>(),
                services.GetRequiredService<DausesDataStore>(),
                services.GetRequiredService<CreditsDataStore>(),
                services.GetRequiredService<LicenseDataStore>());
        }
        private static UsersListViewModel CreateUserListingViewModel(IServiceProvider services)
        {
            return UsersListViewModel.LoadViewModel(
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<UsersDataStore>(),
                services.GetRequiredService<EmployeeStore>(),
                services.GetRequiredService<AuthenticationStore>());
        }
        private static HomeViewModel CreateHomeListingViewModel(IServiceProvider services)
        {
            return HomeViewModel.LoadViewModel(
                services.GetRequiredService<PlayersDataStore>(),
                services.GetRequiredService<PlayersAttendenceStore>(),
                services.GetRequiredService<EmployeeStore>(),
                services.GetRequiredService<NavigationStore>(),

                services.GetRequiredService<SubscriptionDataStore>(),
                services.GetRequiredService<SportDataStore>(),
                services.GetRequiredService<PaymentDataStore>(),
                services.GetRequiredService<MetricDataStore>(),
                services.GetRequiredService<RoutineDataStore>(),
                services.GetRequiredService<LicenseDataStore>(),
                services.GetRequiredService<NavigationService<PlayerListViewModel>>());
        }
    }
}
