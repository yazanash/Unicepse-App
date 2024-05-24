using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.WPF.navigation.Stores;
using Unicepse.WPF.utlis.common;

namespace Unicepse.WPF.navigation
{
    public class NavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigator;

        public readonly Func<TViewModel> _CreateViewModel;

        public NavigationService(NavigationStore navigator, Func<TViewModel> CreateViewModel)
        {
            _navigator = navigator;
            _CreateViewModel = CreateViewModel;
        }



        public void Navigate()
        {
            _navigator.CurrentViewModel?.Dispose();
            _navigator.CurrentViewModel = _CreateViewModel();
        }
        public void ReNavigate()
        {
            _navigator.CurrentViewModel = _CreateViewModel();
        }
    }
}
