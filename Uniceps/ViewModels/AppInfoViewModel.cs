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
using Uniceps.SystemServices;
using Uniceps.utlis.common;
using Uniceps.Views;

namespace Uniceps.ViewModels
{
    public class AppInfoViewModel : ListingViewModelBase
    {
        private static readonly string currentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        private readonly DataExportStore _dataExportStore;
        public AppInfoViewModel(DataExportStore dataExportStore)
        {
            Version = currentVersion;
            _dataExportStore = dataExportStore;
            LoadProfile();
        }
        private bool _hasProfilePicture = false;
        public bool HasProfilePicture 
        {
            get { return _hasProfilePicture; }
            set { _hasProfilePicture = value; OnPropertyChanged(nameof(HasProfilePicture)); }
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
            OnPropertyChanged(nameof(ProfilePicture));
        }
        private void LoadProfile()
        {
            if (!string.IsNullOrEmpty(SettingsManager.Current.LogoPath))
            {
                LoadProfileImage(SettingsManager.Current.LogoPath);
            }
        }
        public ICommand UploadProfilePictureCommand => new RelayCommand(ExecuteUploadProfilePictureCommand);

        public ICommand BackupAndRestore => new RelayCommand(ExecuteOpenBackup);

        private void ExecuteOpenBackup()
        {
            BackupAndRestoreViewModel backupAndRestoreViewModel = new BackupAndRestoreViewModel(_dataExportStore);
            BackupAndRestoreViewWindow backupAndRestoreViewWindow = new BackupAndRestoreViewWindow();
            backupAndRestoreViewWindow.DataContext = backupAndRestoreViewModel;
            backupAndRestoreViewWindow.ShowDialog();
        }
        private string? _version;
        public string? Version
        {
            get { return _version; }
            set { _version = value; OnPropertyChanged(nameof(Version)); }
        }
        public string? Name
        {
            get => SettingsManager.Current.GymName;
            set { SettingsManager.Current.GymName = value ?? ""; OnPropertyChanged(nameof(Name)); }
        }

        public string? Phone
        {
            get => SettingsManager.Current.ContactNumber;
            set { SettingsManager.Current.ContactNumber = value ?? ""; OnPropertyChanged(nameof(Phone)); }
        }

        public string? OwnerName
        {
            get => SettingsManager.Current.OwnerName;
            set { SettingsManager.Current.OwnerName = value ?? ""; OnPropertyChanged(nameof(OwnerName)); }
        }
        public int BackupRemainderDays
        {
            get => SettingsManager.Current.BackupRemainderDays;
            set { SettingsManager.Current.BackupRemainderDays = value; OnPropertyChanged(nameof(BackupRemainderDays)); }
        }
        public int SubscriptionRemainderDays
        {
            get => SettingsManager.Current.SubscriptionRemainderDays;
            set { SettingsManager.Current.SubscriptionRemainderDays = value; OnPropertyChanged(nameof(SubscriptionRemainderDays)); }
        }
        public int SubscriptionRemainderExpirationDays
        {
            get => SettingsManager.Current.SubscriptionRemainderExpirationDays;
            set { SettingsManager.Current.SubscriptionRemainderExpirationDays = value; OnPropertyChanged(nameof(SubscriptionRemainderExpirationDays)); }
        }

        public ICommand UpdateProfileCommand => new RelayCommand(() =>
        {
            SettingsManager.Save();
            MessageBox.Show("تم حفظ البيانات بنجاح في AppData");
        });
      
        private void ExecuteUploadProfilePictureCommand()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog { Filter = "Image Files|*.png;*.jpg;*.jpeg" };
            if (dlg.ShowDialog() == true)
            {
                IsLoading = true;
                try
                {
                    string mediaFolder = Path.Combine(SettingsManager.AppDataPath, "Media");
                    if (!Directory.Exists(mediaFolder)) Directory.CreateDirectory(mediaFolder);

                    string destPath = Path.Combine(mediaFolder, "gym_logo" + Path.GetExtension(dlg.FileName));

                    File.Copy(dlg.FileName, destPath, true);

                    SettingsManager.Current.LogoPath = destPath;
                    SettingsManager.Save();

                    LoadProfileImage(destPath);
                }
                finally { IsLoading = false; }
                 
            }
        }
        public string LastBackupTime => SettingsManager.Current.LastBackupDate == null
            ? "لم يتم إجراء نسخ احتياطي بعد"
            : SettingsManager.Current.LastBackupDate.Value.ToString("g");

        private BitmapImage? _profilePicture;
        public BitmapImage? ProfilePicture
        {
            get { return _profilePicture; }
            set { _profilePicture = value; OnPropertyChanged(nameof(ProfilePicture)); }
        }

    }
}
