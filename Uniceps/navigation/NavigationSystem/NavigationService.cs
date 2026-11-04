using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.navigation.Extensions;
using Uniceps.ViewModels;

namespace Uniceps.navigation.NavigationSystem
{
    public class NavigationService : INavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;

        private readonly Stack<(Type ViewModelType, object? Parameter)> _backStack = new();

        public NavigationService(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            _serviceProvider = serviceProvider;
        }

        public void Navigate<TViewModel>(IDictionary<string, object>? parameters = null, bool addToStack = true) where TViewModel : ViewModelBase
        {
            var targetType = typeof(TViewModel);

            if (addToStack && _navigationStore.CurrentViewModel != null)
            {
                _backStack.Push((_navigationStore.CurrentViewModel.GetType(), null));
            }

            var viewModel = (ViewModelBase)_serviceProvider.GetRequiredService(targetType);

            if (viewModel is IQueryAttributable attributable && parameters != null)
            {
                attributable.ApplyQueryAttributes(parameters);
            }

            _navigationStore.CurrentViewModel?.Dispose();
            _navigationStore.CurrentViewModel = viewModel;
        }

        public void GoBack()
        {
            if (_backStack.Count > 0)
            {
                var previous = _backStack.Pop();

                ViewModelBase viewModel;
                if (previous.Parameter != null)
                {
                    viewModel = (ViewModelBase)ActivatorUtilities.CreateInstance(_serviceProvider, previous.ViewModelType, previous.Parameter);
                }
                else
                {
                    viewModel = (ViewModelBase)_serviceProvider.GetRequiredService(previous.ViewModelType);
                }

                _navigationStore.CurrentViewModel?.Dispose();
                _navigationStore.CurrentViewModel = viewModel;
            }
        }

        public bool CanGoBack => _backStack.Count > 0;
    }
}
