using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels
{
    public class SearchPlayerWindowViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;


        public ViewModelBase? CurrentPrint => _navigatorStore.CurrentViewModel;

        public SearchPlayerWindowViewModel(ListingViewModelBase viewModelBase, NavigationStore navigatorStore)
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
