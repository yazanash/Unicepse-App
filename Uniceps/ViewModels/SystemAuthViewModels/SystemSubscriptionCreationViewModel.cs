using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.SystemAuthCommands;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.SystemAuthViewModels
{
    public class SystemSubscriptionCreationViewModel:ViewModelBase
    {
        private readonly SystemSubscriptionStore _systemSubscriptionStore;
        private readonly ObservableCollection<SystemPlanModelListItemViewModel> _systemPlanModelListItemViewModels;
        public IEnumerable<SystemPlanModelListItemViewModel> SystemPlans => _systemPlanModelListItemViewModels;

        private readonly ObservableCollection<FeatureListItemViewModel> _featureListItemViewModels;
        public IEnumerable<FeatureListItemViewModel> Features => _featureListItemViewModels;

        public SystemSubscriptionCreationViewModel(SystemSubscriptionStore systemSubscriptionStore)
        {
            _systemSubscriptionStore = systemSubscriptionStore;
            _systemPlanModelListItemViewModels = new ObservableCollection<SystemPlanModelListItemViewModel>();
            _featureListItemViewModels = new ObservableCollection<FeatureListItemViewModel>();
            for(int i = 0; i < 5; i++)
            {
                _featureListItemViewModels.Add(new FeatureListItemViewModel($"Feature-{i}"));

            }
            _systemSubscriptionStore.Loaded += _systemSubscriptionStore_Loaded;
            LoadSystemPlansCommand = new LoadSystemPlansCommand(_systemSubscriptionStore);
            CreateSystemSubscriptionCommand = new CreateSystemSubscriptionCommand(_systemSubscriptionStore,this);

        }
        public ICommand LoadSystemPlansCommand { get; }
        public ICommand CreateSystemSubscriptionCommand { get; }
        private SystemPlanModelListItemViewModel? _selectedPlan;
        public SystemPlanModelListItemViewModel? SelectedPlan
        {
            get { return _selectedPlan; }
            set { _selectedPlan = value; OnPropertyChanged(nameof(SelectedPlan)); }
        }
        private void _systemSubscriptionStore_Loaded()
        {
            _systemPlanModelListItemViewModels.Clear();
            foreach(SystemPlanModel systemPlan in _systemSubscriptionStore.SystemPlanModels)
            {
                foreach(var item in systemPlan.PlanItems)
                _systemPlanModelListItemViewModels.Add(new SystemPlanModelListItemViewModel(systemPlan, item));
            }
        }
        public static SystemSubscriptionCreationViewModel LoadViewModel(SystemSubscriptionStore systemSubscriptionStore)
        {
            SystemSubscriptionCreationViewModel systemSubscriptionCreationViewModel = new(systemSubscriptionStore);
            systemSubscriptionCreationViewModel.LoadSystemPlansCommand.Execute(null);
            return systemSubscriptionCreationViewModel;
        }
    }
}
