using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.utlis.common;

namespace Unicepse.navigation.Navigator
{
    public enum ViewType
    {
        Home,
        Players,
        Expenses,
        Accounting,
        Sport,
        Trainer,
        Users,
        About
    }
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }

        //ICommand UpdateCurrentViewModelCommand { get; }
    }
}
