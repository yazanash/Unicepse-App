using System;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.Stores;
using Uniceps.Core.Models.Player;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class AddPlayerViewModel : ErrorNotifyViewModelBase
    {
        private readonly PlayersDataStore _playerStore;
        public int PlayerId;
        public bool IsEditMode;
        public AddPlayerViewModel(PlayersDataStore playerStore)
        {
            _playerStore = playerStore;
            SubmitCommand = new SubmitCommand(this, _playerStore);
        }
        public AddPlayerViewModel(Player player,PlayersDataStore playerStore)
        {
            PlayerId= player.Id;
            FullName = player.FullName;
            Phone = player.Phone;
            Year = player.BirthDate;
            GenderMale = player.GenderMale;
            SubscribeDate = player.SubscribeDate;
            MediclStatus = player.MediclStatus;
            IsEditMode = true;
            _playerStore = playerStore;
            SubmitCommand = new SubmitCommand(this, _playerStore);
        }
        public Action? PlayerCreated;
        internal void OnPlayerCreated()
        {
            PlayerCreated?.Invoke();
        }

        internal void ClearForm()
        {
            FullName = "";
            Phone = "";

        }

        private bool _scanAvailable = true;
        public bool ScanAvailable
        {
            get => _scanAvailable;
            set { _scanAvailable = value; OnPropertyChanged(nameof(ScanAvailable)); }
        }

        public bool UIDCatched => !string.IsNullOrEmpty(UID);


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

        private int _year;
        public int Year
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
        private DateTime _subscribeDate = DateTime.Now.Date;
        public DateTime SubscribeDate
        {
            get { return _subscribeDate; }
            set { _subscribeDate = value; OnPropertyChanged(nameof(SubscribeDate)); }
        }

        public ICommand? OpenScanCommand { get; }

        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }

        private string? _mediclStatus;
        public string? MediclStatus
        {
            get { return _mediclStatus; }
            set { _mediclStatus = value; OnPropertyChanged(nameof(MediclStatus)); }
        }
        #endregion
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
