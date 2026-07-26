using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.Commands.MetricsCommand
{
    public class LoadMetricsCommand : AsyncCommandBase
    {
        private readonly MetricDataStore _metricDataStore;
        private readonly ListingViewModelBase _metricListing;


        public LoadMetricsCommand(ListingViewModelBase metricListing, MetricDataStore metricDataStore)
        {
            _metricDataStore = metricDataStore;
            _metricListing = metricListing;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _metricListing.ErrorMessage = null;
            _metricListing.IsLoading = true;

            try
            {
                if(parameter is int playerId)
                await _metricDataStore.GetAllByPlayer(playerId);
            }
            catch (Exception)
            {
                _metricListing.ErrorMessage = "خطأ في تحميل قياسات اللاعب يرجى اعادة تشغيل البرنامج";
            }
            finally
            {
                _metricListing.IsLoading = false;
            }
        }
    }
}
