using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.ViewModels.PlayersViewModels;

namespace Unicepse.Commands.MetricsCommand
{
    public class LoadMetricsCommand : AsyncCommandBase
    {
        private readonly MetricDataStore _metricDataStore;
        private readonly ListingViewModelBase _metricListing;
        private readonly PlayerListItemViewModel _player;


        public LoadMetricsCommand(ListingViewModelBase metricListing, MetricDataStore metricDataStore, PlayerListItemViewModel player)
        {
            _metricDataStore = metricDataStore;
            _metricListing = metricListing;
            _player = player;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _metricListing.ErrorMessage = null;
            _metricListing.IsLoading = true;

            try
            {

                await _metricDataStore.GetAll(_player.Player);
            }
            catch (Exception)
            {
                _metricListing.ErrorMessage = "Failed to load Players. Please restart the application.";
            }
            finally
            {
                _metricListing.IsLoading = false;
            }
        }
    }
}
