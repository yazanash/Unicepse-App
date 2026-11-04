using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;
using Uniceps.MessengerSystem;
using Uniceps.MessengerSystem.Events;

namespace Uniceps.BioEngineServices
{
    public class BioEngine
    {
        private readonly DeviceScannerService _scannerService;
        private readonly BiometricLogicService _logicService;
        private readonly BiometricStorageService _biometricStorageService;

        public event Action<int, int>? OnUserFound;
        public event Action? OnUserNotFound;
        public event Action<bool, byte[], Player>? OnUserSaved;

        public event Action? OnDeviceConnected;
        public event Action? OnDeviceDisconnected;
        public event Action<int>? OnEnrollStepCaptured;
        public event Action<string>? OnEnrollFailed;

        public event Action? OnVerifyMode;
        public BioEngine(DeviceScannerService scannerService, BiometricLogicService logicService, BiometricStorageService biometricStorageService)
        {
            _scannerService = scannerService;
            _logicService = logicService;
            _biometricStorageService = biometricStorageService;

            Messenger.Default.Register<StartEnrollmentMessage>(this, msg => EnrollNewUser(msg.Player));
            Messenger.Default.Register<StopEnrollmentMessage>(this, msg => StopEnrollNewUser());
            _logicService.DbHandle = _biometricStorageService.GetDbHandle();
            _scannerService.OnDeviceConnected += ()=> OnDeviceConnected?.Invoke();
            _scannerService.OnDeviceDisconnected += () => OnDeviceDisconnected?.Invoke();
            _scannerService.OnFingerprintCaptured += _scannerService_OnFingerprintCaptured;
            _logicService.OnFingerprintPassedForVerification += _logicService_OnFingerprintPassedForVerification;
            _logicService.OnMasterTemplateGenerated += _logicService_OnMasterTemplateGenerated;
            _logicService.OnEnrollStepCaptured += (step) => OnEnrollStepCaptured?.Invoke(step);
            _logicService.OnEnrollFailed +=(message) => OnEnrollFailed?.Invoke(message);
            RegisterEngineEvents();

        }

        private void StopEnrollNewUser()
        {
            _logicService.SetVerificationMode();
        }

        private void RegisterEngineEvents()
        {
            OnUserFound += (playerId, score) =>
            {
                Messenger.Default.Send(new UserFingerprintScannedMessage(playerId, score));
            };

            OnEnrollStepCaptured += (step) =>
            {
                Messenger.Default.Send(new EnrollmentStepMessage(step, $"Step {step} captured"));
            };

            OnUserSaved += (inserted, template, user) =>
            {
                    Messenger.Default.Send(new EnrollmentCompletedMessage(user, template));
            };


            OnEnrollFailed += (reason) =>
            {
                Messenger.Default.Send(new EnrollmentFailedMessage(reason));
            };
        }
        public void StartDevice()
        {
            _scannerService.Start();
        }
        public void EnrollNewUser(Player enrollmentQueue)
        {
            _logicService.SetEnrollmentMode(enrollmentQueue);
        }
        public void LoadUsersToRam(List<Player> users)
        {
            _biometricStorageService.LoadFingerprintsIntoRam(users);
        }
        public void StopDevice()
        {
            _scannerService.Stop();
            _biometricStorageService.Close();
        }
        private void _logicService_OnMasterTemplateGenerated(byte[] arg1, Player queue )
        {
            try
            {
                bool inserted = _biometricStorageService.AddSingleUserToRam(arg1, queue);
                OnUserSaved?.Invoke(inserted, arg1, queue);
            }
            catch (Exception ex)
            {
                OnEnrollFailed?.Invoke($"خطأ أثناء الحفظ المحلي: {ex.Message}");
            }
            finally
            {
                _logicService.SetVerificationMode();
                OnVerifyMode?.Invoke();
            }
        }

        private void _logicService_OnFingerprintPassedForVerification(byte[] rawBytes)
        {
            int score = 0;
            int playerId = _biometricStorageService.Verify(rawBytes, ref score);

            if (playerId != -1)
                OnUserFound?.Invoke(playerId, score);
            else
                OnUserNotFound?.Invoke();
        }

        private void _scannerService_OnFingerprintCaptured(byte[] obj)
        {
            _logicService.HandleIncomingFingerprint(obj);
        }

       
    }
}
