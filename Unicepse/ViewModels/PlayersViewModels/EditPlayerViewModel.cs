using Unicepse.Core.Models;
using Unicepse.Core.Models.Player;
using Unicepse.Commands;
using Unicepse.Commands.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.PlayersViewModels
{
    public class EditPlayerViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly NavigationStore _navigationStore;
        private readonly PlayersDataStore _playerStore;
        public ObservableCollection<Year> years;
        public IEnumerable<Year> Years => years;
        #region Properties
        public int Id { get; }

        private string? _fullName;
        public string? FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
                ClearError(nameof(FullName));
                if (string.IsNullOrEmpty(FullName?.Trim()))
                {
                    AddError("هذا الحقل مطلوب", nameof(FullName));
                    OnErrorChanged(nameof(Phone));
                }
            }
        }
        private string? _phone;
        public string? Phone
        {
            get { return _phone; }
            set
            {
                _phone = value; OnPropertyChanged(nameof(Phone));
                ClearError(nameof(Phone));
                if (Phone?.Trim().Length < 10)
                {
                    AddError("يجب ان يكون رقم الهاتف 10 ارقام", nameof(Phone));
                    OnErrorChanged(nameof(Phone));
                }

            }
        }

        private Year? _year;
        public Year? Year
        {
            get { return _year; }
            set
            {
                _year = value;

                OnPropertyChanged(nameof(Year));
            }
        }
        private bool _genderMale;
        public bool GenderMale
        {
            get { return _genderMale; }
            set { _genderMale = value; OnPropertyChanged(nameof(GenderMale)); }
        }
        private double _weight;
        public double Weight
        {
            get { return _weight; }
            set
            {
                _weight = value; OnPropertyChanged(nameof(Weight));
                ClearError(nameof(Weight));
                if (string.IsNullOrEmpty(Weight.ToString()))
                {
                    AddError("لا يمكن ان يكون هذا الحقل فارغا", nameof(Weight));
                    OnErrorChanged(nameof(Weight));
                }
            }
        }
        private double _hieght;
        public double Hieght
        {
            get { return _hieght; }
            set
            {
                _hieght = value; OnPropertyChanged(nameof(Hieght));
                ClearError(nameof(Hieght));
                if (string.IsNullOrEmpty(Hieght.ToString()))
                {
                    AddError("لا يمكن ان يكون هذا الحقل فارغا", nameof(Hieght));
                    OnErrorChanged(nameof(Hieght));
                }
            }
        }
        private DateTime _subscribeDate = DateTime.Now.Date;
        public DateTime SubscribeDate
        {
            get { return _subscribeDate; }
            set { _subscribeDate = value; OnPropertyChanged(nameof(SubscribeDate)); }
        }


        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        #endregion




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
        }




        public readonly Dictionary<string, List<string>> PropertyNameToErrorsDictionary;

        public EditPlayerViewModel(NavigationStore navigationStore, PlayersDataStore playerStore, PlayerMainPageViewModel playerProfileViewModel)
        {
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            years = new ObservableCollection<Year>();
            for (int i = DateTime.Now.Year - 80; i < DateTime.Now.Year; i++)
                years.Add(new Year() { year = i });
            _navigationStore = navigationStore;
            _playerStore = playerStore;
            //_subscriptionDataStore = subscriptionDataStore;
            //_paymentDataStore = paymentDataStore;
            //_playerProfileViewModel = playerProfileViewModel;
            Id = _playerStore.SelectedPlayer!.Id;
            FullName = _playerStore.SelectedPlayer!.FullName;
            Phone = _playerStore.SelectedPlayer!.Phone;
            Year = years.SingleOrDefault(x => x.year == _playerStore.SelectedPlayer!.BirthDate);
            GenderMale = _playerStore.SelectedPlayer!.GenderMale;
            Weight = _playerStore.SelectedPlayer!.Weight;
            Hieght = _playerStore.SelectedPlayer!.Hieght;
            SubscribeDate = _playerStore.SelectedPlayer!.SubscribeDate;
            //_sportStore = sportStore;    
            //_licenseDataStore = licenseDataStore;
            ScanAvailable = false;
            SubmitCommand = new EditPlayerCommand(new NavigationService<PlayerMainPageViewModel>(_navigationStore, () => playerProfileViewModel), this, _playerStore, _navigationStore);
            CancelCommand = new NavaigateCommand<PlayerMainPageViewModel>(new NavigationService<PlayerMainPageViewModel>(_navigationStore, () => playerProfileViewModel));
        
        }
        private static PlayerMainPageViewModel CreatePlayerProfileViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore, SportDataStore sportDataStore,LicenseDataStore licenseDataStore)
        {
            return PlayerMainPageViewModel.LoadViewModel(navigatorStore, subscriptionDataStore, playersDataStore, paymentDataStore, sportDataStore,licenseDataStore);
        }
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => PropertyNameToErrorsDictionary.Any();

        public bool CanSubmit => !HasErrors;
        private bool? _submited = false;
        public bool ScanAvailable { get; set; }
        public bool? Submited
        {
            get { return _submited; }
            set
            {
                _submited = value;
                OnPropertyChanged(nameof(Submited));

            }
        }
        private string? _submitMessage;
        public string? SubmitMessage
        {
            get { return _submitMessage; }
            set
            {
                _submitMessage = value;
                OnPropertyChanged(nameof(SubmitMessage));

            }
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return PropertyNameToErrorsDictionary!.GetValueOrDefault(propertyName, new List<string>());
        }


    }
}
