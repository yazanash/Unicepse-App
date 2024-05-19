using PlatinumGym.Core.Models;
using PlatinumGymPro.Commands.AccountantCommand;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.Accountant
{
    public class MounthlyReportViewModel : ListingViewModelBase
    {
        private readonly GymStore _gymStore;
        public MounthlyReportItemViewModel MounthlyReportItemViewModel { get; set; }
        public MounthlyReportViewModel(GymStore gymStore)
        {
            _gymStore = gymStore;
            MounthlyReportItemViewModel = new MounthlyReportItemViewModel(new MonthlyReport());
            gymStore.ReportLoaded += GymStore_ReportLoaded;
            GetReport = new GetReportCommand(_gymStore, this);
        }

        private void GymStore_ReportLoaded(MonthlyReport obj)
        {
            //MounthlyReportItemViewModel.Update(obj);
            MounthlyReportItemViewModel = new(obj);
            OnPropertyChanged(nameof(MounthlyReportItemViewModel));

        }
        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }
        public ICommand GetReport { get; }
    }
}
