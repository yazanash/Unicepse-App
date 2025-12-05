using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.utlis.common;
namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class ExportDialogViewModel:ViewModelBase
    {
        private readonly RoutineTempDataStore _routineTempDataStore;
        public ObservableCollection<FileFormatType> FileFormats { get; set; } = new();
        public ICommand ChangeFolderPathCommand { get; }
        public ICommand? ExportCommand { get; }
        public Action? RoutineExported;
        public void OnRoutineExported()
        {
            RoutineExported?.Invoke();

        }
        public ExportDialogViewModel(RoutineTempDataStore routineTempDataStore)
        {
            _routineTempDataStore = routineTempDataStore;
            FileName = routineTempDataStore.SelectedRoutine?.Name;
            foreach (var item in Enum.GetValues(typeof(FileFormatType)))
            {
                FileFormats.Add((FileFormatType)item);
            }
            string savedPath = Properties.Settings.Default.ExportFolderPath;
            if (string.IsNullOrEmpty(savedPath))
            {
                string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                Properties.Settings.Default.ExportFolderPath = defaultPath;
                Properties.Settings.Default.Save();

                FolderPath = defaultPath;
            }
            else
                FolderPath = savedPath;

            ChangeFolderPathCommand = new RelayCommand(SelectAndSaveFolder);
            ExportCommand = new ExportRoutineCommand(this,_routineTempDataStore);
        }
        public void SelectAndSaveFolder()
        {
            var dialog = new OpenFolderDialog
            {
                Title = "اختر مجلد لحفظ ملفات التصدير"
            };

            if (dialog.ShowDialog() == true)
            {
                Properties.Settings.Default.ExportFolderPath = dialog.FolderName;
                Properties.Settings.Default.Save();
                FolderPath = dialog.FolderName;
            }
        }

        private string? _fileName;

		public string? FileName
		{
			get { return _fileName; }
			set { _fileName = value; OnPropertyChanged(nameof(FileName)); }
		}

        private string? _folderPath;
        public string? FolderPath
        {
            get { return _folderPath; }
            set { _folderPath = value; OnPropertyChanged(nameof(FolderPath)); }
        }
        private FileFormatType _fileFormat;
        public FileFormatType FileFormat
        {
            get { return _fileFormat; }
            set { _fileFormat = value; OnPropertyChanged(nameof(FileFormat)); }
        }
    }
}
