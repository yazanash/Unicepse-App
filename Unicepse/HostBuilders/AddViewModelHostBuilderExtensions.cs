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

namespace Unicepse.HostBuilders
{
    public static class AddViewModelHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<AuthViewModel>();
                services.AddSingleton<LicenseViewModel>();
                services.AddSingleton<HomeNavViewModel>();
                //services.AddSingleton<PlayersPageViewModel>(); SportListViewModel
                services.AddTransient((s)=> CreatePlayerListingViewModel(s));
                services.AddTransient((s) => CreateSportListingViewModel(s));
                services.AddSingleton<SportsViewModel>();
                services.AddSingleton<TrainersViewModel>();
                services.AddSingleton<UsersViewModel>();



                services.AddSingleton<AccountingViewModel>();
                services.AddSingleton<HomeNavViewModel>();
                services.AddSingleton<INavigator,Navigator>();
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
                services.GetRequiredService<LicenseDataStore>()
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
    }
}
