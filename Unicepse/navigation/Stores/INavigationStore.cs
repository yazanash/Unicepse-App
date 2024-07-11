using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;

namespace Unicepse.navigation.Stores
{
    public interface INavigationStore
    {
        ViewModelBase CurrentViewModel { get; }
        public event Action? CurrentViewModelChanged;
    }
}
