using Uniceps.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.Commands.Sport;
using Uniceps.navigation;
using Uniceps.ViewModels;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.Core.Models.Sport;

namespace Uniceps.ViewModels.SportsViewModels
{
    public class AddSportViewModel : ListingViewModelBase, INotifyDataErrorInfo
    {
        private readonly SportDataStore _sportStore;
        public Action? SportCreated;
        public void OnSportCreated()
        {
            SportCreated?.Invoke();
        }
        public Sport? Sport;
        public bool IsEditMode;
        public AddSportViewModel(SportDataStore sportStore)
        {
            _sportStore = sportStore;
            SubmitCommand = new SubmitSportCommand(this, _sportStore);
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            _sportStore = sportStore;
            WeeklyTrainingDays = 6;
            SubscribeLength = 30;
        }
        public AddSportViewModel(SportDataStore sportStore, Sport sport)
        {
            _sportStore = sportStore;
            SubmitCommand = new SubmitSportCommand(this, _sportStore);
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            _sportStore = sportStore;
            WeeklyTrainingDays = 6;
            SubscribeLength = 30;
            Sport = sport;
            IsEditMode = true;
            SportName = Sport.Name;
            MonthlyPrice = Sport.Price;
            WeeklyTrainingDays = Sport.DaysInWeek;
            SubscribeLength = Sport.DaysCount;
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

        private void AddError(string? ErrorMsg, string? propertyName)
        {
            if (!PropertyNameToErrorsDictionary.ContainsKey(propertyName!))
            {
                PropertyNameToErrorsDictionary.Add(propertyName!, new List<string>());

            }
            PropertyNameToErrorsDictionary[propertyName!].Add(ErrorMsg!);
            OnErrorChanged(propertyName);
        }

        private void ClearError(string? propertyName)
        {
            PropertyNameToErrorsDictionary.Remove(propertyName!);
            OnErrorChanged(propertyName);
        }

        private void OnErrorChanged(string? PropertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(PropertyName));
            OnPropertyChanged(nameof(CanSubmit));
        }
        public bool CanSubmit => !HasErrors;
       
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

        public readonly Dictionary<string, List<string>> PropertyNameToErrorsDictionary;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => PropertyNameToErrorsDictionary.Any();

        public IEnumerable GetErrors(string? propertyName)
        {
            return PropertyNameToErrorsDictionary!.GetValueOrDefault(propertyName, new List<string>());
        }

        internal void ClearForm()
        {
            SportName = "";
            MonthlyPrice = 0;
        }
    }
}
