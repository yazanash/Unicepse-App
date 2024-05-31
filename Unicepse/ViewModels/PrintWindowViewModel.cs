using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.navigation.Stores;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels
{
    public class PrintWindowViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;


        public ViewModelBase? CurrentPrint => _navigatorStore.CurrentViewModel;

        public PrintWindowViewModel(ViewModelBase viewModelBase, NavigationStore navigatorStore)
        {
            _navigatorStore = navigatorStore;
            _navigatorStore.CurrentViewModel = viewModelBase;
            _navigatorStore.CurrentViewModelChanged += _navigatorStore_CurrentViewModelChanged; ;
        }

        private void _navigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentPrint));
        }
    }
}
