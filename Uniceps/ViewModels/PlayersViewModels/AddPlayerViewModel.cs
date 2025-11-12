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
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Player;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class AddPlayerViewModel : ErrorNotifyViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly PlayersDataStore _playerStore;
        private readonly PlayerProfileViewModel _playerProfileViewModel;
        public ObservableCollection<Year> years;

        public IEnumerable<Year> Years => years;
        public AddPlayerViewModel(NavigationStore navigationStore, PlayerListViewModel playerListViewModel, PlayersDataStore playerStore, PlayerProfileViewModel playerProfileViewModel)
        {
            years = new ObservableCollection<Year>();
            for (int i = DateTime.Now.Year - 80; i < DateTime.Now.Year; i++)
                years.Add(new Year() { year = i });
            Year = years.SingleOrDefault(x => x.year == DateTime.Now.Year - 1);


            _navigationStore = navigationStore;
            _playerStore = playerStore;

            _playerProfileViewModel = playerProfileViewModel;

            _playerStore.profile_loaded += _playerStore_profile_loaded;
            ScanAvailable = true;
            OpenScanCommand = new GetProfileScanCommand(new ReadPlayerQrCodeViewModel(), _playerStore);
            CancelCommand = new NavaigateCommand<PlayerListViewModel>(new NavigationService<PlayerListViewModel>(_navigationStore, () => playerListViewModel));
            NavigationStore PlayerMainPageNavigation = new NavigationStore();
            SubmitCommand = new SubmitCommand(new NavigationService<PlayerProfileViewModel>(_navigationStore, () => _playerProfileViewModel), this, _playerStore);
        }

        private void _playerStore_profile_loaded(Profile obj)
        {
            if (obj != null)
            {
                FullName = obj.FullName;
                Phone = obj.Phone;
                GenderMale = obj.GenderMale;
                Year = years.FirstOrDefault(x => x.year == obj.BirthDate);
                UID = obj.UID;
            }
        }
        public bool ScanAvailable { get; set; }
        private static PlayerProfileViewModel CreatePlayerProfileViewModel(NavigationStore navigatorStore,
            SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, SportDataStore sportDataStore,
            PaymentDataStore paymentDataStore, MetricDataStore metricStore,
            PlayersAttendenceStore playersAttendenceStore,   NavigationService<PlayerListViewModel> navigationService,
            ExercisesDataStore exercisesDataStore)
        {
            return new PlayerProfileViewModel(navigatorStore, subscriptionDataStore, playersDataStore, sportDataStore, paymentDataStore, metricStore,
                playersAttendenceStore,  navigationService, exercisesDataStore);
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
                    OnErrorChanged(nameof(FullName));
                }
            }
        }
        private string? _uid;
        public string? UID
        {
            get { return _uid; }
            set
            {
                _uid = value;
                OnPropertyChanged(nameof(UID));
            }
        }
        private string? _phone = "0";
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

        public ICommand? OpenScanCommand { get; }

        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        #endregion

    }
}
