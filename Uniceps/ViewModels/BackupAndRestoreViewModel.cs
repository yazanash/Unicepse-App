using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.DataExporter;
using Uniceps.Stores.ApiDataStores;

namespace Uniceps.ViewModels
{
    public class BackupAndRestoreViewModel : ViewModelBase
    {
        private readonly DataExportStore _dataExportStore;
        public int Progress { get; set; }
        public string? ProgressMessage { get; set; }
        private string? _folderPath;
        public string? FolderPath
        {
            get { return _folderPath; }
            set { _folderPath = value; OnPropertyChanged(nameof(FolderPath)); }
        }
        private string? _filePath;
        public string? FilePath
        {
            get { return _filePath; }
            set { _filePath = value; OnPropertyChanged(nameof(FilePath)); }
        }
        public BackupAndRestoreViewModel(DataExportStore dataExportStore)
        {
            _dataExportStore = dataExportStore;
            _dataExportStore.ProgressChanged += _dataExportStore_ProgressChanged;
            _dataExportStore.StatusChanged += _dataExportStore_StatusChanged;
            _dataExportStore.ExportFinished += _dataExportStore_ExportFinished;
            _dataExportStore.ImportFinished += _dataExportStore_ImportFinished;
            FolderPath = Properties.Settings.Default.BackupPath;
        }

        private void _dataExportStore_ImportFinished(bool arg1, string arg2)
        {
            if (arg1)
            {
                MessageBox.Show(arg2+"...."+ "سيتم اغلاق التطبيق ... قم باعادة تشغيله لتطبيق التحديثات");
                Application.Current.Shutdown();
            }
        }

        public ICommand ChangeFolderPathCommand => new RelayCommand(SelectAndSaveFolder);
        public void SelectAndSaveFolder()
        {
            var dialog = new OpenFolderDialog
            {
                Title = "اختر مجلد لحفظ ملفات التصدير"
            };

            if (dialog.ShowDialog() == true)
            {
                Properties.Settings.Default.BackupPath = dialog.FolderName;
                Properties.Settings.Default.Save();
                FolderPath = dialog.FolderName;
            }
        }
        private void _dataExportStore_ExportFinished(bool arg1, string arg2)
        {
            if (arg1)
            {
                MessageBox.Show(arg2);
            }
        }

        private void _dataExportStore_StatusChanged(string obj)
        {
            ProgressMessage = obj;
            OnPropertyChanged(nameof(ProgressMessage));
        }

        private void _dataExportStore_ProgressChanged(int obj)
        {
            Progress = obj;
            OnPropertyChanged(nameof(Progress));
        }

        public ICommand ExportBackup => new AsyncRelayCommand(ExecuteExportBackup);

        public async Task ExecuteExportBackup()
        {
            if (!string.IsNullOrEmpty(FolderPath))
                await _dataExportStore.StartExportAsync();
            else
                MessageBox.Show("يرجى اختار مسار الملف اولا");
           
        }
        public ICommand ChooseImportBackupCommand => new RelayCommand(SelectAndSaveFile);
        public void SelectAndSaveFile()
        {
            var dialog = new OpenFileDialog { Filter = "Uniceps Backup (*.unxpro)|*.unxpro" };
            if (dialog.ShowDialog() == true)
            {
                if (dialog.FileName.EndsWith(".unxpro"))
                {
                    FilePath = dialog.FileName;
                }
                else
                    MessageBox.Show("هذا الملف غير مدعوم");
            }
        }
        public ICommand ImportBackup => new AsyncRelayCommand(ExecuteImportBackup);

        public async Task ExecuteImportBackup()
        {
            if (!string.IsNullOrEmpty(FilePath))
                await _dataExportStore.StartImportAsync(FilePath);
            else
                MessageBox.Show("يرجى اختار الملف اولا");
        }
    }
}
