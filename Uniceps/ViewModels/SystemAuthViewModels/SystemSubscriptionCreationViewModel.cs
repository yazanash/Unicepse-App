
using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.SystemAuthCommands;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.navigation;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.ViewModels.SystemAuthViewModels
{
    public class FeatureItem
    {
        public string Name { get; set; } = "";
        public bool IsFree { get; set; } = false;
        public bool IsPremium { get; set; } = true;
        public string FreeLimitString { get; set; } = "";
        public string PrimumLimitString { get; set; } = "";
    }
    public class PlansConfig
    {
        public List<FeatureItem> Features { get; set; } = new();
    }
    public class SystemSubscriptionCreationViewModel : ViewModelBase
    {
        private readonly SystemSubscriptionStore _systemSubscriptionStore;
        private readonly ObservableCollection<SystemPlanModelListItemViewModel> _systemPlanModelListItemViewModels;
        public IEnumerable<SystemPlanModelListItemViewModel> SystemPlans => _systemPlanModelListItemViewModels;

        private readonly ObservableCollection<FeatureListItemViewModel> _featureListItemViewModels;
        public IEnumerable<FeatureListItemViewModel> Features => _featureListItemViewModels;
        private NavigationService<SubscriptionMainViewModel> _navigationService;
        public SystemSubscriptionCreationViewModel(SystemSubscriptionStore systemSubscriptionStore, NavigationService<SubscriptionMainViewModel> navigationService)
        {
            _systemSubscriptionStore = systemSubscriptionStore;
            _navigationService = navigationService;

            _systemPlanModelListItemViewModels = new ObservableCollection<SystemPlanModelListItemViewModel>();
            _featureListItemViewModels = new ObservableCollection<FeatureListItemViewModel>();
            var assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream("Uniceps.Resources.plans.json");
            if (stream != null)
            {
                using StreamReader reader = new StreamReader(stream);
                string json = reader.ReadToEnd();
                var config = JsonConvert.DeserializeObject<PlansConfig>(json);
                if (config != null)
                {
                    var features = config.Features;
                    int id = 1;
                    foreach (var feature in features)
                    {
                        _featureListItemViewModels.Add(new FeatureListItemViewModel(feature,id++));
                    }
                }
              
            }
          
           
            _systemSubscriptionStore.Loaded += _systemSubscriptionStore_Loaded;
            LoadSystemPlansCommand = new LoadSystemPlansCommand(_systemSubscriptionStore);
            CreateSystemSubscriptionCommand = new CreateSystemSubscriptionCommand(_systemSubscriptionStore, this, _navigationService);
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
            foreach (SystemPlanModel systemPlan in _systemSubscriptionStore.SystemPlanModels)
            {
                foreach (var item in systemPlan.PlanItems)
                    _systemPlanModelListItemViewModels.Add(new SystemPlanModelListItemViewModel(systemPlan, item));
            }
        }
        public Action? SubscriptionRequested;
        public void OnSubscriptionRequested()
        {
            SubscriptionRequested?.Invoke();
        }
        public static SystemSubscriptionCreationViewModel LoadViewModel(SystemSubscriptionStore systemSubscriptionStore, NavigationService<SubscriptionMainViewModel> navigationService)
        {
            SystemSubscriptionCreationViewModel systemSubscriptionCreationViewModel = new(systemSubscriptionStore, navigationService);
            systemSubscriptionCreationViewModel.LoadSystemPlansCommand.Execute(null);
            return systemSubscriptionCreationViewModel;
        }
    }
}
