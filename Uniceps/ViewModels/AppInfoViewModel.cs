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
using Uniceps.Core.Models;
using Uniceps.DataExporter;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.Views;

namespace Uniceps.ViewModels
{
    public class AppInfoViewModel : ListingViewModelBase
    {
        private static readonly string currentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        private AccountStore _accountStore;
        public event Action? Profile_Updated;
        private readonly DataExportStore _dataExportStore;
        public string LastBackupTime => Properties.Settings.Default.LastBackup == DateTime.MinValue
        ? "لم يتم إجراء نسخ احتياطي بعد"
        : Properties.Settings.Default.LastBackup.ToString("g");
        public AppInfoViewModel(AccountStore accountStore, DataExportStore dataExportStore)
        {

            Version = currentVersion;
            _accountStore = accountStore;
            //_accountStore.ProfileChanged += _accountStore_ProfileChanged;
         

            LoadProfile();
          
            _dataExportStore = dataExportStore;
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
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(localPath);
            bitmap.EndInit();
            bitmap.Freeze(); 
            ProfilePicture = bitmap;
            HasProfilePicture = true;
        }
        public ICommand UploadProfilePictureCommand => new AsyncRelayCommand(ExecuteUploadProfilePictureCommand);
        public ICommand? UpdateProfileCommand { get; set; }

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
                    await Task.Delay(0);
                    IsLoading = false;
                    MessageBox.Show("تم تحديث الصورة بنجاح");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Upload error: " + ex.Message);
                }
            }
        }
        private void LoadProfile()
        {
           
        }

        private void _accountStore_ProfileChanged()
        {
            LoadProfile();
        }

        internal void OnProfileUpdated()
        {
            Profile_Updated?.Invoke();
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
