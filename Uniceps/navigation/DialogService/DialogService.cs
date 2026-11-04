using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.navigation.Extensions;

namespace Uniceps.navigation.DialogService
{
    public class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool? ShowDialog<TWindow, TViewModel>(IDictionary<string, object>? parameters = null)
          where TWindow : Window, new()
          where TViewModel : class
        {
            var window = new TWindow();
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            if (viewModel is IQueryAttributable attributable && parameters != null)
            {
                attributable.ApplyQueryAttributes(parameters);
            }
            window.DataContext = viewModel;
            return window.ShowDialog();
        }

    }
}
