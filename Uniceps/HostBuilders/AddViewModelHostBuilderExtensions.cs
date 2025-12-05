using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Uniceps.Views.AuthView;
using Microsoft.Extensions.Logging;
using Uniceps.Stores;
using Uniceps.navigation;
using Uniceps.ViewModels.RoutineTemplateViewModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.Stores.EmployeeStores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.Authentication;
using Uniceps.ViewModels.Accountant;
using Uniceps.Stores.SportStores;
using Uniceps.navigation.Navigator;
using Uniceps.Views;
using Uniceps.ViewModels.SystemAuthViewModels;
using Uniceps.ViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.HostBuilders
{
    public static class AddViewModelHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<AuthViewModel>();
                services.AddSingleton<AppInfoViewModel>();
                services.AddSingleton<SplashScreenViewModel>();
                services.AddSingleton<SystemProfileCreationViewModel>();
                services.AddSingleton<SystemLoginViewModel>();
                services.AddTransient<RoutineItemListViewModel>();
                services.AddTransient<RoutineItemsBufferListViewModel>();
                services.AddSingleton<RoutineDayGroupViewModel>();
                services.AddSingleton<RoutineDetailsViewModel>();
                services.AddSingleton<SetModelItemsListViewModel>(); 
                services.AddSingleton<Func<RoutineDetailsViewModel>>((s) => () => s.GetRequiredService<RoutineDetailsViewModel>());
                services.AddSingleton<NavigationService<RoutineDetailsViewModel>>();
                services.AddTransient((s) => CreateRoutineListingViewModel(s));
                services.AddTransient((s) => CreateHomeListingViewModel(s));
                services.AddTransient((s) => CreatePlayerListingViewModel(s));
                services.AddTransient((s) => CreateSportListingViewModel(s));
                services.AddTransient((s) => CreateEmployeeListingViewModel(s));
                services.AddTransient((s) => CreateUserListingViewModel(s));
                services.AddTransient((s) => CreatePlayerProfileViewModel(s));
                services.AddTransient((s) => CreateExercisesViewModel(s));
                services.AddTransient((s) => CreateSubscriptionListingViewModel(s));
                services.AddSingleton<AccountingViewModel>();
                services.AddTransient<Func<PlayerListViewModel>>(services => () => services.GetRequiredService<PlayerListViewModel>());
                services.AddTransient<NavigationService<PlayerListViewModel>>();
                services.AddSingleton<INavigator, Navigator>();

                services.AddTransient<Func<RoutineListViewModel>>(services => () => services.GetRequiredService<RoutineListViewModel>());
                services.AddTransient<NavigationService<RoutineListViewModel>>();
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
                    DataContext = s.GetRequiredService<SystemLoginViewModel>(),
                });
            });
            return _hostBuilder;
        }
        private static PlayerListViewModel CreatePlayerListingViewModel(IServiceProvider services)
        {
            return PlayerListViewModel.LoadViewModel(
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<PlayersDataStore>(),
                services.GetRequiredService<ILogger<PlayerListViewModel>>(),
                services.GetRequiredService<PlayerProfileViewModel>()
                );
        }
        private static SportListViewModel CreateSportListingViewModel(IServiceProvider services)
        {
            return SportListViewModel.LoadViewModel(
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<SportDataStore>(),
                services.GetRequiredService<EmployeeStore>(),
                services.GetRequiredService<SportSubscriptionDataStore>());
        }
        private static TrainersListViewModel CreateEmployeeListingViewModel(IServiceProvider services)
        {
            return TrainersListViewModel.LoadViewModel(
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<EmployeeStore>(),
                services.GetRequiredService<SportDataStore>(),
                services.GetRequiredService<DausesDataStore>(),
                services.GetRequiredService<CreditsDataStore>(),
                services.GetRequiredService<EmployeeSubscriptionDataStore>(),
                 services.GetRequiredService<AccountStore>());
        }
        private static UsersListViewModel CreateUserListingViewModel(IServiceProvider services)
        {
            return UsersListViewModel.LoadViewModel(
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<UsersDataStore>(),
                services.GetRequiredService<AuthenticationStore>());
        }
        private static ExercisesListViewModel CreateExercisesViewModel(IServiceProvider services)
        {
            return ExercisesListViewModel.LoadViewModel(
                services.GetRequiredService<ExercisesDataStore>(),
                services.GetRequiredService<DayGroupDataStore>(),
                services.GetRequiredService<RoutineItemDataStore>(),
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<NavigationService<RoutineDetailsViewModel>>(),
                  services.GetRequiredService < RoutineItemsBufferListViewModel > ());
        }
        private static HomeViewModel CreateHomeListingViewModel(IServiceProvider services)
        {
            return HomeViewModel.LoadViewModel(services.GetRequiredService<PlayersAttendenceStore>());
        }
        private static SubscriptionMainViewModel CreateSubscriptionListingViewModel(IServiceProvider services)
        {
            return SubscriptionMainViewModel.LoadViewModel(
                services.GetRequiredService<SubscriptionDataStore>(),
                 services.GetRequiredService<PlayersDataStore>(),
                  services.GetRequiredService<SportDataStore>(),
                   services.GetRequiredService<PaymentDataStore>(),
                   services.GetRequiredService<EmployeeStore>(), 
                   services.GetRequiredService<PlayersAttendenceStore>(),
                   services.GetRequiredService<AccountStore>());
        }
        private static RoutineListViewModel CreateRoutineListingViewModel(IServiceProvider services)
        {
            return RoutineListViewModel.LoadViewModel(
                services.GetRequiredService<RoutineTempDataStore>(),
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<RoutineDetailsViewModel>());
        }
        private static PlayerProfileViewModel CreatePlayerProfileViewModel(IServiceProvider services)
        {
            return new PlayerProfileViewModel(
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<SubscriptionDataStore>(),
                services.GetRequiredService<PlayersDataStore>(),
                services.GetRequiredService<SportDataStore>(),
                services.GetRequiredService<PaymentDataStore>(),
                services.GetRequiredService<MetricDataStore>(),
                services.GetRequiredService<PlayersAttendenceStore>(),
                services.GetRequiredService<NavigationService<PlayerListViewModel>>(),
                services.GetRequiredService<ExercisesDataStore>(),
                 services.GetRequiredService<AccountStore>(),
                   services.GetRequiredService<EmployeeStore>());
        }
    }
}
