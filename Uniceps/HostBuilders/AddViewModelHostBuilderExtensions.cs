using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Uniceps.navigation.DialogService;
using Uniceps.navigation.NavigationSystem;
using Uniceps.navigation.Navigator;
using Uniceps.utlis.ComponentsViewModels;
using Uniceps.ViewModels;
using Uniceps.ViewModels.Accountant;
using Uniceps.ViewModels.Authentication;
using Uniceps.ViewModels.DashboardViewModels;
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.Expenses;
using Uniceps.ViewModels.Metrics;
using Uniceps.ViewModels.OtherRevenueViewModels;
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
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<INavigator, Navigator>();

                services.AddSingleton<IDialogService, DialogService>();


                services.AddSingleton<MainWindow>(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>(),
                });
                services.AddSingleton<AuthWindow>(s => new AuthWindow()
                {
                    DataContext = s.GetRequiredService<LoginViewModel>(),
                });
                services.AddTransient<MainWindowViewModel>();
                services.AddTransient<LoginViewModel>();
                services.AddTransient<AppInfoViewModel>();
                services.AddTransient<SplashScreenViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<DashboardViewModel>();
                services.AddTransient<PremiumViewModel>();
                services.AddTransient<SystemSubscriptionPaymentOptionDialogViewModel>();
                services.AddTransient<AuthenticationLoggingList>();
                services.AddTransient<StatusBarViewModel>();

                /// players
                services.AddTransient<AddPlayerViewModel>();
                services.AddTransient<PlayerProfileViewModel>();
                services.AddTransient<PlayerMainPageViewModel>();
                services.AddTransient<PlayerListViewModel>();

                /// payments
                services.AddTransient<PaymentListViewModel>();
                services.AddTransient<AddPaymentViewModel>();

                // metrics 
                services.AddTransient<MetricReportViewModel>();
                services.AddTransient<AddMetricsViewModel>();

                //attendance
                services.AddTransient<PlayerAttendenceViewModel>();

                //subscriptions
                services.AddTransient<SubscriptionMainViewModel>();
                services.AddTransient<CreateSubscriptionWindowViewModel>();

                //sports
                services.AddTransient<SportListViewModel>();
                services.AddTransient<AddSportViewModel>();
                services.AddTransient<SportSubscriptionsViewModel>();


                //employees 
                services.AddTransient<TrainersListViewModel>();
                services.AddTransient<AddEmployeeViewModel>();
                services.AddTransient<EmployeeAccountViewModel>();
                services.AddTransient<EmployeeAccountantPageViewModel>();
                services.AddTransient<CreateCreditViewModelWindow>();
                services.AddTransient<CreditListViewModel>();


                //users
                services.AddTransient<AddUserViewModel>();
                services.AddTransient<UsersListViewModel>();
                
                
                //accountant
                services.AddTransient<AccountingViewModel>();
                services.AddTransient<SubscriptionStateCardViewModel>();
                services.AddTransient<CreditsCardViewModel>();
                services.AddTransient<PaymentsCardViewModel>();
                services.AddTransient<ExpensesCardViewModel>();
                services.AddTransient<AccountingStateViewModel>();
                services.AddTransient<ExpensesListViewModel>();
                services.AddTransient<RevenueListViewModel>();
                services.AddTransient<ExpensesReportViewModel>();
                services.AddTransient<IncomeReportViewModel>();
                services.AddTransient<MounthlyReportViewModel>();




                //routine
                services.AddTransient<ExportDialogViewModel>();
                services.AddTransient<RoutineItemListViewModel>();
                services.AddTransient<RoutineItemsBufferListViewModel>();
                services.AddTransient<RoutineDayGroupViewModel>();
                services.AddTransient<RoutineDetailsViewModel>();
                services.AddTransient<SetModelItemsListViewModel>();
                services.AddTransient<RoutineListViewModel>();
                services.AddTransient<ExercisesListViewModel>();
            });
            return _hostBuilder;
        }

    }
}
