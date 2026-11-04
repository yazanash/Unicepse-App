using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Core.Models.Player;
using Uniceps.navigation.Extensions;
using Uniceps.Stores;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class EnrollFingerprintViewModel:ViewModelBase,IQueryAttributable
    {
        private readonly PlayersDataStore _playersDataStore;
        private string? _statusMessage;
        public string? StatusMessage 
        { get => _statusMessage;
            set {  _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }
        public Action? RequestClose;

        private bool _isStep1Active; public bool IsStep1Active { get => _isStep1Active; set { _isStep1Active = value; OnPropertyChanged(nameof(IsStep1Active)); } }
        private bool _isStep2Active; public bool IsStep2Active { get => _isStep2Active; set { _isStep2Active = value; OnPropertyChanged(nameof(IsStep2Active)); } }
        private bool _isStep3Active; public bool IsStep3Active { get => _isStep3Active; set { _isStep3Active = value; OnPropertyChanged(nameof(IsStep3Active)); } }

        public EnrollFingerprintViewModel(PlayersDataStore playersDataStore)
        {
            _playersDataStore = playersDataStore;
            _playersDataStore.EnrollmentStepCaptured += OnEnrollStepCaptured;
            _playersDataStore.EnrollmentFailed += OnEnrollFailed;
            _playersDataStore.EnrollmentCompleted += OnEnrollCompleted;
            StartEnrollment();
        }
        public ICommand StartEnrollCommand => new RelayCommand(StartEnrollment);

        public Player? CurrentPlayer { get; set; }
        public void StartEnrollment()
        {
            if (CurrentPlayer != null)
            {
                StatusMessage = "الرجاء وضع الإصبع على الجهاز...";
                _playersDataStore.RequestFingerprintEnrollment(CurrentPlayer);
            }
        }

        private void OnEnrollStepCaptured(int step)
        {
            StatusMessage = $"تم التقاط البصمة {step}";
            if (step >= 1) IsStep1Active = true;
            if (step >= 2) IsStep2Active = true;
            if (step >= 3) IsStep3Active = true;
        }

        private void OnEnrollFailed(string reason)
        {
            StatusMessage = $"فشلت عملية التسجيل: {reason}";
        }

        private void OnEnrollCompleted(Player player)
        {
            StatusMessage = "تم تسجيل البصمة وحفظ البيانات بنجاح!";
            RequestClose?.Invoke();
        }

        public override void Dispose()
        {
            _playersDataStore.EnrollmentStepCaptured -= OnEnrollStepCaptured;
            _playersDataStore.EnrollmentFailed -= OnEnrollFailed;
            _playersDataStore.EnrollmentCompleted -= OnEnrollCompleted;
            base.Dispose();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<Player>("Player", out Player? player))
            {
                if (player != null)
                    CurrentPlayer = player;
            }

        }
    }
}
