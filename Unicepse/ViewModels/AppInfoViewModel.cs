using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Unicepse.Commands.LicenseCommand;
using Unicepse.Core.Models;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.ViewModels._ِAppViewModels;

namespace Unicepse.ViewModels
{
    public class AppInfoViewModel : ListingViewModelBase
    {
        private readonly LicenseDataStore _licenseDataStore;
        private readonly ObservableCollection<LicensesListItemViewModel> _licensesListItemViewModels;
        private static readonly string currentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        public IEnumerable<LicensesListItemViewModel> Licenses => _licensesListItemViewModels;
        public AppInfoViewModel(LicenseDataStore licenseDataStore)
        {
            _licenseDataStore = licenseDataStore;
            _licensesListItemViewModels = new ObservableCollection<LicensesListItemViewModel>();
            _licenseDataStore.Loaded += _licenseDataStore_Loaded;

            LoadLicensesCommand = new LoadLicensesCommand(_licenseDataStore, this);
            Version = currentVersion;
            if (_licenseDataStore.CurrentGymProfile != null)
            {
                GymName = _licenseDataStore.CurrentGymProfile!.GymName;
                GymOwner = _licenseDataStore.CurrentGymProfile!.OwnerName;
                GymPhone = _licenseDataStore.CurrentGymProfile!.PhoneNumber;
                GymTelephone = _licenseDataStore.CurrentGymProfile!.Telephone;
                GymAddress = _licenseDataStore.CurrentGymProfile!.Address;
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
           


        }
        public ICommand LoadLicensesCommand { get; }
        private void _licenseDataStore_Loaded()
        {
            _licensesListItemViewModels.Clear();
            foreach(var item in _licenseDataStore.Licenses)
            {
                AddLicense(item);
            }
        }
        void AddLicense(License license)
        {
            LicensesListItemViewModel licensesListItem = new LicensesListItemViewModel(license);
            _licensesListItemViewModels.Add(licensesListItem);
        }
        private string? _version;
        public string? Version
        {
            get { return _version; }
            set { _version = value; OnPropertyChanged(nameof(Version)); }
        }
        private string? _gymName;
        public string? GymName
        {
            get { return _gymName; }
            set { _gymName = value; OnPropertyChanged(nameof(GymName)); }
        }
        private string? _gymOwner;
        public string? GymOwner
        {
            get { return _gymOwner; }
            set { _gymOwner = value; OnPropertyChanged(nameof(GymOwner)); }
        }
        private string? _gymPhone;
        public string? GymPhone
        {
            get { return _gymPhone; }
            set { _gymPhone = value; OnPropertyChanged(nameof(GymPhone)); }
        }
        private string? _gymTelephone;
        public string? GymTelephone
        {
            get { return _gymTelephone; }
            set { _gymTelephone = value; OnPropertyChanged(nameof(GymTelephone)); }
        }
        private string? _gymAddress;

        public string? GymAddress
        {
            get { return _gymAddress; }
            set { _gymAddress = value; OnPropertyChanged(nameof(GymAddress)); }
        }

        private BitmapImage? _gymLogo;

        public BitmapImage? GymLogo
        {
            get { return _gymLogo; }
            set { _gymLogo = value; OnPropertyChanged(nameof(GymLogo)); }
        }

        public static AppInfoViewModel LoadViewModel(LicenseDataStore licenseDataStore)
        {
            AppInfoViewModel viewModel = new(licenseDataStore);

            viewModel.LoadLicensesCommand.Execute(null);

            return viewModel;
        }
    }
}
