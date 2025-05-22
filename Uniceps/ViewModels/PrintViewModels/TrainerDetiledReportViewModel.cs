using Uniceps.Commands.Employee;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Core.Models.Employee;

namespace Uniceps.ViewModels.PrintViewModels
{
    public class TrainerDetiledReportViewModel : ViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        public CollectionViewSource GroupedTasks { get; set; }
        public IEnumerable<SubscriptionListItemViewModel> Subscriptions => _subscriptionListItemViewModels;
        public EmployeeAccountantPageViewModel _employeeAccountantPageViewModel;
        public PrintedTrainerMounthlyReportViewModel? TrainerMounthlyReportViewModel { get; set; }
        private readonly LicenseDataStore _licenseDataStore;
        public TrainerDetiledReportViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, EmployeeAccountantPageViewModel employeeAccountantPageViewModel, LicenseDataStore licenseDataStore)
        {
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _dausesDataStore.StateChanged += _dausesDataStore_StateChanged;
            _employeeAccountantPageViewModel = employeeAccountantPageViewModel;
            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            GroupedTasks = new CollectionViewSource();
            ReportDate = _employeeAccountantPageViewModel.ReportDate.ToShortDateString();
            FullName = _employeeStore.SelectedEmployee!.FullName;
            _licenseDataStore = licenseDataStore;
            GymName = _licenseDataStore.CurrentGymProfile!.GymName;
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_licenseDataStore.CurrentGymProfile!.Logo!);
                bitmap.EndInit();
                GymLogo = bitmap;
            }
            catch
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/Assets/logo.png");
                bitmap.EndInit();
                GymLogo = bitmap;
            }
        }
        private string? _gymName;
        public string? GymName
        {
            get { return _gymName; }
            set { _gymName = value; OnPropertyChanged(nameof(GymName)); }
        }
        private BitmapImage? _gymLogo;

        public BitmapImage? GymLogo
        {
            get { return _gymLogo; }
            set { _gymLogo = value; OnPropertyChanged(nameof(GymLogo)); }
        }
        private void _dausesDataStore_StateChanged(TrainerDueses? obj)
        {
            TrainerMounthlyReportViewModel = new(obj!);
            _subscriptionListItemViewModels.Clear();
            foreach (var item in _employeeAccountantPageViewModel.SubscriptionsList)
            {
                _subscriptionListItemViewModels.Add(item);
            }
            GroupedTasks.Source = _subscriptionListItemViewModels;
            GroupedTasks.GroupDescriptions.Add(new PropertyGroupDescription("SportName"));
        }

        private string? _fullName;
        public string? FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }
        private string? _reportDate;
        public string? ReportDate
        {
            get { return _reportDate; }
            set { _reportDate = value; OnPropertyChanged(nameof(ReportDate)); }
        }

    }
}
