using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels;

namespace Uniceps.navigation.Stores
{
    public interface INavigationStore
    {
        ViewModelBase CurrentViewModel { get; }
        public event Action? CurrentViewModelChanged;
    }
}
