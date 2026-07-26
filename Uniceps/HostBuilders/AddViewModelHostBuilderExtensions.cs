using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Uniceps.navigation;
using Uniceps.navigation.Navigator;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.Accountant;
using Uniceps.ViewModels.Authentication;
using Uniceps.ViewModels.DashboardViewModels;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.Metrics;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.ViewModels.PlayersAttendenceViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views;
using Uniceps.Views.AuthView;

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
                services.AddTransient<RoutineItemListViewModel>();
                services.AddTransient<RoutineItemsBufferListViewModel>();
                services.AddSingleton<RoutineDayGroupViewModel>();
                services.AddSingleton<RoutineDetailsViewModel>();
                services.AddSingleton<SetModelItemsListViewModel>();
                services.AddSingleton<Func<RoutineDetailsViewModel>>((s) => () => s.GetRequiredService<RoutineDetailsViewModel>());
                services.AddSingleton<NavigationService<RoutineDetailsViewModel>>();

                services.AddTransient((s) => CreateRoutineListingViewModel(s));
                services.AddTransient<HomeViewModel>();
                services.AddTransient<PlayerListViewModel>(provider =>
                new PlayerListViewModel(
                    provider.GetRequiredService<NavigationStore>(),
                    provider.GetRequiredService<PlayersDataStore>(),
                    provider.GetRequiredService<ILogger<PlayerListViewModel>>(),
                    provider.GetRequiredService<Func<int, PlayerProfileViewModel>>()
                ));

                services.AddTransient<SportListViewModel>();
                services.AddTransient<TrainersListViewModel>();
                services.AddTransient((s) => CreateUserListingViewModel(s));
                services.AddSingleton<Func<int, PlayerProfileViewModel>>(provider => playerId =>
    CreatePlayerProfileViewModel(provider, playerId));

                services.AddSingleton<Func<int, PlayerMainPageViewModel>>(provider => playerId =>
                new PlayerMainPageViewModel(
                    playerId,
                    provider.GetRequiredService<NavigationStore>(),
                    provider.GetRequiredService<SubscriptionDataStore>(),
                    provider.GetRequiredService<PlayersDataStore>(),
                    provider.GetRequiredService<PaymentDataStore>(),
                    provider.GetRequiredService<SportDataStore>(),
                    provider.GetRequiredService<EmployeeStore>(),
                    provider.GetRequiredService<Func<int, CreateSubscriptionWindowViewModel>>())
                );

                services.AddSingleton<Func<int, CreateSubscriptionWindowViewModel>>(provider => playerId =>
              new CreateSubscriptionWindowViewModel(
                  playerId,
                  provider.GetRequiredService<SportDataStore>(),
                  provider.GetRequiredService<SubscriptionDataStore>(),
                  provider.GetRequiredService<PlayersDataStore>(),
                  provider.GetRequiredService<PaymentDataStore>(),
                  provider.GetRequiredService<EmployeeStore>())
              );

                services.AddSingleton<Func<int, PaymentListViewModel>>(provider => playerId =>
            new PaymentListViewModel(
                playerId,
                provider.GetRequiredService<PaymentDataStore>(),
                provider.GetRequiredService<SubscriptionDataStore>())
            );

                services.AddSingleton<Func<int, MetricReportViewModel>>(provider => playerId=>
                new MetricReportViewModel(
                    playerId,
            provider.GetRequiredService<MetricDataStore>(),
             provider.GetRequiredService<NavigationStore>())
        );

                services.AddSingleton<Func<int, PlayerAttendenceViewModel>>(provider => playerId =>
             new PlayerAttendenceViewModel(
                 playerId,
         provider.GetRequiredService<PlayersAttendenceStore>())
     );
                services.AddSingleton<Func<PremiumViewModel>>(provider => ()=>
             new PremiumViewModel()
     );

                services.AddTransient((s) => CreateExercisesViewModel(s));
                services.AddTransient<SubscriptionMainViewModel>();
                services.AddTransient((s) => CreateDashboard(s));
                services.AddSingleton<AccountingViewModel>();
                services.AddTransient<Func<PlayerListViewModel>>(services => () => services.GetRequiredService<PlayerListViewModel>());
                services.AddTransient<NavigationService<PlayerListViewModel>>();
                services.AddSingleton<INavigator, Navigator>();
                services.AddTransient<SystemSubscriptionPaymentOptionDialogViewModel>();
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
            });
            return _hostBuilder;
        }
        private static DashboardViewModel CreateDashboard(IServiceProvider services)
        {
            return DashboardViewModel.LoadViewModel(
                services.GetRequiredService<GymStore>()
                );
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
                  services.GetRequiredService<RoutineItemsBufferListViewModel>());
        }
        private static RoutineListViewModel CreateRoutineListingViewModel(IServiceProvider services)
        {
            return RoutineListViewModel.LoadViewModel(
                services.GetRequiredService<RoutineTempDataStore>(),
                services.GetRequiredService<NavigationStore>(),
                services.GetRequiredService<RoutineDetailsViewModel>());
        }
        private static PlayerProfileViewModel CreatePlayerProfileViewModel(IServiceProvider services, int playerId)
        {
            return new PlayerProfileViewModel(
         playerId,
         services.GetRequiredService<PlayersDataStore>(),
         services.GetRequiredService<LicenseStore>(),
         services.GetRequiredService<Func<int, PlayerMainPageViewModel>>(),
         services.GetRequiredService<Func<int, PaymentListViewModel>>(),
         services.GetRequiredService<Func<int, MetricReportViewModel>>(),
         services.GetRequiredService<Func<int, PlayerAttendenceViewModel>>(),
         services.GetRequiredService<Func<PremiumViewModel>>()
     );
        }
    }
}
