using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels;

namespace Uniceps.navigation.NavigationSystem
{
    public interface INavigationService
    {
        void Navigate<TViewModel>(IDictionary<string, object>? parameters = null, bool addToStack = true) where TViewModel : ViewModelBase;
        void GoBack();
        bool CanGoBack { get; }
    }
}
