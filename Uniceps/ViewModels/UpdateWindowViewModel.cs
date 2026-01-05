using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.BackgroundServices;
using Uniceps.Commands;

namespace Uniceps.ViewModels
{
    public class UpdateWindowViewModel : ViewModelBase
    {
        public ReleaseDto Release;

        public UpdateWindowViewModel(ReleaseDto release)
        {
            Release = release;
        }
        public string AppVersion => Release.Version;
        public string ChangeLog => Release.ChangeLogAr;

        public ICommand UpdateCommand => new RelayCommand(ExecuteUpdate);
        private void ExecuteUpdate()
        {
            Updater.OpenDownloadPage(Release.Id);
        }
    }
}
