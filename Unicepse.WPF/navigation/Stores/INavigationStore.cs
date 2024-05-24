using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.WPF.utlis.common;

namespace Unicepse.WPF.navigation.Stores
{
    public interface INavigationStore
    {
        ViewModelBase CurrentViewModel { get; }
        public event Action? CurrentViewModelChanged;
    }
}
