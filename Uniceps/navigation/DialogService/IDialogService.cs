using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Uniceps.navigation.DialogService
{
    public interface IDialogService
    {
        bool? ShowDialog<TWindow, TViewModel>(IDictionary<string, object>? parameters = null)
         where TWindow : Window, new()
         where TViewModel : class;
    }
}
