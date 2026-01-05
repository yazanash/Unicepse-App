using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Uniceps.Commands;
using Uniceps.Commands.SystemAuthCommands;
using Uniceps.Core.Models;
using Uniceps.DataExporter;
using Uniceps.Stores;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.utlis.common;
using Uniceps.Views;

namespace Uniceps.ViewModels
{
    public class AppInfoViewModel : ListingViewModelBase
    {
        private static readonly string currentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        private AccountStore _accountStore;
        private IProfileDataStore _systemProfileStore;
        public event Action? ProfileUpdated;
        private readonly DataExportStore _dataExportStore;
        private readonly ISystemAuthStore _systemAuthStore;
        public string LastBackupTime => Properties.Settings.Default.LastBackup == DateTime.MinValue
        ? "لم يتم إجراء نسخ احتياطي بعد"
        : Properties.Settings.Default.LastBackup.ToString("g");
        public AppInfoViewModel(AccountStore accountStore, IProfileDataStore systemProfileStore, DataExportStore dataExportStore, ISystemAuthStore systemAuthStore)
        {

            Version = currentVersion;
            _accountStore = accountStore;
            _accountStore.ProfileChanged += _accountStore_ProfileChanged;
            _accountStore.SubscriptionChanged += _accountStore_SubscriptionChanged;
            _systemProfileStore = systemProfileStore;

            LoadProfile();
            LoadSubscription();
            UpdateProfileCommand = new UpdateProfileCommand(systemProfileStore, this);
            _dataExportStore = dataExportStore;
            _systemAuthStore = systemAuthStore;
        }
        private bool _hasProfile = false;
        public bool HasProfile
        {
            get => _hasProfile;
            set
            {
                _hasProfile = value;
                OnPropertyChanged(nameof(HasProfile));
            }
        }
        private bool _hasProfilePicture = false;
        public bool HasProfilePicture
        {
            get => _hasProfilePicture;
            set
            {
                _hasProfilePicture = value;
                OnPropertyChanged(nameof(HasProfilePicture));
            }
        }
        private void LoadProfileImage(string localPath)
        {
            if (!File.Exists(localPath))
                return;

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // مهم لتجنب قفل الملف
            bitmap.UriSource = new Uri(localPath);
            bitmap.EndInit();
            bitmap.Freeze(); // إذا ستستخدمه من thread آخر
            ProfilePicture = bitmap;
            HasProfilePicture = true;
        }
        public ICommand UploadProfilePictureCommand => new AsyncRelayCommand(ExecuteUploadProfilePictureCommand);
        public ICommand UpdateProfileCommand { get; set; }

        public ICommand LogoutCommand => new RelayCommand(ExecuteLogout);

        private void ExecuteLogout()
        {
            if(MessageBox.Show("سيتم تسجيل الخروج من الحساب على هذا الجهاز ... سيتم اغلاق التطبيق , هل انت متاكد ؟","تنويه"
                ,MessageBoxButton.YesNo,MessageBoxImage.Warning)== MessageBoxResult.Yes)
            {
                _systemAuthStore.Logout();
                Application.Current.Shutdown();
            }
        }

        public ICommand BackupAndRestore => new RelayCommand(ExecuteOpenBackup);

        private void ExecuteOpenBackup()
        {
            BackupAndRestoreViewModel backupAndRestoreViewModel = new BackupAndRestoreViewModel(_dataExportStore);
            BackupAndRestoreViewWindow backupAndRestoreViewWindow = new BackupAndRestoreViewWindow();
            backupAndRestoreViewWindow.DataContext = backupAndRestoreViewModel;
            backupAndRestoreViewWindow.ShowDialog();
        }

        private async Task ExecuteUploadProfilePictureCommand()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";

            if (dlg.ShowDialog() == true)
            {
                string localFilePath = dlg.FileName;
                try
                {
                    IsLoading = true;
                     await _systemProfileStore.UploadProfilePicture(localFilePath);
                    IsLoading = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Upload error: " + ex.Message);
                }
            }
        }
        private void LoadProfile()
        {
            if (_accountStore.SystemProfile != null)
            {
                Name = _accountStore.SystemProfile.DisplayName;
                Phone = _accountStore.SystemProfile.PhoneNumber;
                OwnerName = _accountStore.SystemProfile.OwnerName;
                Address = _accountStore.SystemProfile.Address;
                if (!string.IsNullOrEmpty( _accountStore.SystemProfile.LocalProfileImagePath))
                {
                    LoadProfileImage(_accountStore.SystemProfile.LocalProfileImagePath);
                }
                HasProfile = true;
            }
            else
            {
                Name = "غير محدد";
                Phone = "غير محدد";
                OwnerName = "غير محدد";
                Address = "غير محدد";
                HasProfile = false;
            }
        }
        private void LoadSubscription()
        {
            if (_accountStore.SystemSubscription != null)
            {
                PlanName = _accountStore.SystemSubscription.PlanName;
                Price = _accountStore.SystemSubscription.Price;
                StartDate = _accountStore.SystemSubscription.StartDate.ToShortDateString();
                EndDate = _accountStore.SystemSubscription.EndDate.ToShortDateString();
                DaysLeft =Convert.ToInt32( _accountStore.SystemSubscription.EndDate.Subtract(DateTime.Now).TotalDays);
            }
            else
            {
                PlanName = "نسخة تجريبية";
                Price = 0;
                StartDate ="---";
                EndDate = "---";
                DaysLeft = 0;
            }
        }
        private void _accountStore_SubscriptionChanged()
        {
            LoadSubscription();
        }

        private void _accountStore_ProfileChanged()
        {
            LoadProfile();
        }

        internal void OnProfileUpdated()
        {
           ProfileUpdated?.Invoke();
        }

        public ICommand? LoadLicensesCommand { get; }
       
        private string? _version;
        public string? Version
        {
            get { return _version; }
            set { _version = value; OnPropertyChanged(nameof(Version)); }
        }
        private string? _name;
        public string? Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        private string? _ownerName;
        public string? OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; OnPropertyChanged(nameof(OwnerName)); }
        }
        private string? _phone;
        public string? Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }
       
        private string? _address;

        public string? Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(nameof(Address)); }
        }

        private BitmapImage? _profilePicture;

        public BitmapImage? ProfilePicture
        {
            get { return _profilePicture; }
            set { _profilePicture = value; OnPropertyChanged(nameof(ProfilePicture)); }
        }
        private string? _planName;
        public string? PlanName {
            get { return _planName; }
            set { _planName = value; OnPropertyChanged(nameof(PlanName)); }
        }
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged(nameof(Price)); }
        }
        private string? _startDate;
        public string? StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(nameof(StartDate)); }
        }
        private string? _endDate;
        public string? EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(nameof(EndDate)); }
        }
        private int _daysLeft;
        public int DaysLeft
        {
            get { return _daysLeft; }
            set { _daysLeft = value; OnPropertyChanged(nameof(DaysLeft)); }
        }
    }
}
