using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands
{
    public class ExportRoutineCommand : AsyncCommandBase
    {
        private readonly ExportDialogViewModel _exportDialogViewModel;
        private readonly RoutineTempDataStore _routineTempDataStore;
        public ExportRoutineCommand(ExportDialogViewModel exportDialogViewModel, RoutineTempDataStore routineTempDataStore)
        {
            _exportDialogViewModel = exportDialogViewModel;
            _routineTempDataStore = routineTempDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            var folder = Properties.Settings.Default.ExportFolderPath;
            var fileName = _exportDialogViewModel.FileName!;
            if (!string.IsNullOrEmpty(fileName))
            {
                var fullPath = Path.Combine(folder, fileName);
                if (_routineTempDataStore.SelectedRoutine != null)
                {
                  bool exported =  await _routineTempDataStore.ExportRoutine(_routineTempDataStore.SelectedRoutine.Id,
                       _exportDialogViewModel.FileFormat, fullPath);
                    if (exported) {
                        MessageBox.Show("تم التصدير بنجاح");
                        _exportDialogViewModel.OnRoutineExported();

                    }
                    else
                        MessageBox.Show("تم الغاء عملية التصدير");

                }
            }
            else
                MessageBox.Show("يجب تحديد اسم الملف");
                
                
        }
    }
}
