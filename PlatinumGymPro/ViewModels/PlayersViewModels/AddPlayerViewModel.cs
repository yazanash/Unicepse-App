using PlatinumGym.Core.Models.Player;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PlayersViewModels
{
    public class AddPlayerViewModel : ViewModelBase,INotifyDataErrorInfo
    {
        private readonly NavigationStore _navigationStore;
        private readonly PlayersDataStore _playerStore;
        private readonly SportDataStore _sportStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PlayerListViewModel _playerListViewModel;
        public AddPlayerViewModel(NavigationStore navigationStore, PlayerListViewModel playerListViewModel, PlayersDataStore playerStore, SubscriptionDataStore subscriptionDataStore, SportDataStore sportStore, PaymentDataStore paymentDataStore)
        {
            _navigationStore = navigationStore;
            _playerStore = playerStore;
            _subscriptionDataStore = subscriptionDataStore;
            _paymentDataStore = paymentDataStore;
            _sportStore = sportStore;
            _playerListViewModel = playerListViewModel;
            CancelCommand = new NavaigateCommand<PlayerListViewModel>(new NavigationService<PlayerListViewModel>(_navigationStore, () => playerListViewModel));
            this.SubmitCommand = new SubmitCommand(new NavigationService<PlayerProfileViewModel>(_navigationStore, () => CreatePlayerProfileViewModel(navigationStore, _subscriptionDataStore, _playerStore, _sportStore,_paymentDataStore)), this, _playerStore, _navigationStore, _playerListViewModel, _subscriptionDataStore, _sportStore);
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
        }

        private static PlayerProfileViewModel CreatePlayerProfileViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore,PlayersDataStore playersDataStore,SportDataStore sportDataStore,PaymentDataStore paymentDataStore)
        {
            return new PlayerProfileViewModel(navigatorStore, subscriptionDataStore, playersDataStore, sportDataStore, paymentDataStore);
        }
        private bool? _submited = false;
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
        
        private int _birthDate;
        public int BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); }
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
            set { _weight = value; OnPropertyChanged(nameof(Weight));
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
            set { _hieght = value; OnPropertyChanged(nameof(Hieght));
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


        public bool CanSubmit => !HasErrors;

        public readonly Dictionary<string, List<string>> PropertyNameToErrorsDictionary;
       
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

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

        public bool HasErrors => PropertyNameToErrorsDictionary.Any();

        public IEnumerable GetErrors(string? propertyName)
        {
            return PropertyNameToErrorsDictionary!.GetValueOrDefault(propertyName, new List<string>());
        }
    }
}
