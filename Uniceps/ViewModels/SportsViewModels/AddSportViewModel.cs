using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Commands.Sport;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.navigation;
using Uniceps.navigation.Extensions;
using Uniceps.Stores;
using Uniceps.ViewModels;

namespace Uniceps.ViewModels.SportsViewModels
{
    public class AddSportViewModel : ErrorNotifyViewModelBase,IQueryAttributable
    {
        private readonly SportDataStore _sportStore;
        public Action? SportCreated;
        public void OnSportCreated()
        {
            SportCreated?.Invoke();
        }
        public Sport? Sport;
        public bool IsEditMode;
        private void ApplyEdit(Sport sport)
        {
            Sport = sport;
            IsEditMode = true;
            SportName = Sport.Name;
            MonthlyPrice = Sport.Price;
            WeeklyTrainingDays = Sport.DaysInWeek;
            SubscribeLength = Sport.DaysCount;
        }
        public AddSportViewModel(SportDataStore sportStore)
        {
            _sportStore = sportStore;
            SubmitCommand = new SubmitSportCommand(this, _sportStore);
            _sportStore = sportStore;
            WeeklyTrainingDays = 6;
            SubscribeLength = 30;
        }
        public int Id { get; }

        private string? _sportName;
        public string? SportName
        {
            get { return _sportName; }
            set
            {
                _sportName = value;
                OnPropertyChanged(nameof(SportName));
                ClearError(nameof(SportName));
                if (string.IsNullOrEmpty(SportName?.Trim()))
                {
                    AddError("هذا الحقل مطلوب", nameof(SportName));
                    OnErrorChanged(nameof(SportName));
                }
            }
        }
        private double _monthlyPrice;
        public double MonthlyPrice
        {
            get { return _monthlyPrice; }
            set
            {
                _monthlyPrice = value; OnPropertyChanged(nameof(MonthlyPrice));
                ClearError(nameof(MonthlyPrice));
                if (MonthlyPrice < 0)
                {
                    AddError("لايمكن ان تكون القيمة اقل من 0", nameof(MonthlyPrice));
                    OnErrorChanged(nameof(MonthlyPrice));
                }

            }
        }
        private int _weeklyTrainingDays;
        public int WeeklyTrainingDays
        {
            get { return _weeklyTrainingDays; }
            set
            {
                _weeklyTrainingDays = value;
                OnPropertyChanged(nameof(WeeklyTrainingDays));
                ClearError(nameof(WeeklyTrainingDays));
                if (WeeklyTrainingDays == 0 || WeeklyTrainingDays > 7)
                {
                    AddError("لا يمكن ان يكون هذا الرقم اقل من 1 او اكثر من 7", nameof(WeeklyTrainingDays));
                    OnErrorChanged(nameof(WeeklyTrainingDays));
                }
            }
        }
        private int _subscribeLength;
        public int SubscribeLength
        {
            get { return _subscribeLength; }
            set
            {
                _subscribeLength = value;
                OnPropertyChanged(nameof(SubscribeLength));
                ClearError(nameof(SubscribeLength));
                if (SubscribeLength == 0)
                {
                    AddError("لا يمكن ان يكون هذا الرقم اقل من 1", nameof(SubscribeLength));
                    OnErrorChanged(nameof(SubscribeLength));
                }
            }
        }
        public ICommand? SubmitCommand { get; }
        internal void ClearForm()
        {
            SportName = "";
            MonthlyPrice = 0;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<Sport>("Sport", out var sport))
            {
                if(sport!=null) 
                ApplyEdit(sport);
            }
           
        }
    }
}
