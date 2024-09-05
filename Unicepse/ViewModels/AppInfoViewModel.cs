using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Unicepse.Stores;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels
{
    public class AppInfoViewModel : ViewModelBase
    {
        private readonly LicenseDataStore _licenseDataStore;

        public AppInfoViewModel(LicenseDataStore licenseDataStore)
        {
            _licenseDataStore = licenseDataStore;
            if(_licenseDataStore.CurrentGymProfile != null)
            {
                GymName = _licenseDataStore.CurrentGymProfile!.GymName;
                GymOwner = _licenseDataStore.CurrentGymProfile!.OwnerName;
                GymPhone = _licenseDataStore.CurrentGymProfile!.PhoneNumber;
                GymTelephone = _licenseDataStore.CurrentGymProfile!.Telephone;
                GymAddress = _licenseDataStore.CurrentGymProfile!.Address;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_licenseDataStore.CurrentGymProfile!.Logo!);
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
