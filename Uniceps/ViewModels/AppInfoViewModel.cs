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
using Uniceps.Core.Models;
using Uniceps.Stores;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels
{
    public class AppInfoViewModel : ListingViewModelBase
    {
        private static readonly string currentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        private AccountStore _accountStore;
        public AppInfoViewModel(AccountStore accountStore)
        {

            Version = currentVersion;
            _accountStore = accountStore;
            _accountStore.ProfileChanged += _accountStore_ProfileChanged;
            if (_accountStore.SystemProfile != null)
            {
                GymName = _accountStore.SystemProfile.DisplayName;
                GymPhone = _accountStore.SystemProfile.PhoneNumber;
            }
        }

        private void _accountStore_ProfileChanged()
        {
            if (_accountStore.SystemProfile != null)
            {
                GymName = _accountStore.SystemProfile.DisplayName;
                GymPhone = _accountStore.SystemProfile.PhoneNumber;
            }
        }

        public ICommand? LoadLicensesCommand { get; }
       
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

       
    }
}
