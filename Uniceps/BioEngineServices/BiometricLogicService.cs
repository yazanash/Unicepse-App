using libzkfpcsharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;

namespace Uniceps.BioEngineServices
{
    public class BiometricLogicService
    {
        public IntPtr DbHandle { get; set; } = IntPtr.Zero;
        public SystemState CurrentState { get; private set; } = SystemState.Verification;
        private int _enrollStep = 0;
        private readonly byte[][] _tempTemplates = new byte[3][];

        public event Action<byte[]>? OnFingerprintPassedForVerification;
        public event Action<int>? OnEnrollStepCaptured;
        public event Action<byte[], Player>? OnMasterTemplateGenerated;
        public event Action<string>? OnEnrollFailed;
        public Player? EnrollmentQueue = null;
        public void SetVerificationMode()
        {
            CurrentState = SystemState.Verification;
            ResetEnrollBuffers();
        }
        public void SetEnrollmentMode(Player enrollmentQueue)
        {
            CurrentState = SystemState.Enrollment;
            EnrollmentQueue = enrollmentQueue;
            ResetEnrollBuffers();
        }
        public void HandleIncomingFingerprint(byte[] rawTemplate)
        {
            if (CurrentState == SystemState.Verification)
            {
                OnFingerprintPassedForVerification?.Invoke(rawTemplate);
            }
            else if (CurrentState == SystemState.Enrollment)
            {
                ProcessEnrollmentStep(rawTemplate);
            }
        }

        private void ProcessEnrollmentStep(byte[] rawTemplate)
        {
            if (rawTemplate == null || rawTemplate.Length == 0)
            {
                OnEnrollFailed?.Invoke("فشل التقاط الإشارة النية من الحساس.");
                return;
            }

            try
            {
                if (_enrollStep == 0)
                {
                    int matchedPlayerId = CheckIfFingerprintExists(rawTemplate);

                    // إذا وجدنا البصمة مسجلة للاعب آخر (وليس نفس اللاعب الذي نعدل بصمته)
                    if (matchedPlayerId > 0 && (EnrollmentQueue == null || matchedPlayerId != EnrollmentQueue.Id))
                    {
                        OnEnrollFailed?.Invoke($"البصمة مسجلة بالفعل للاعب آخر : {matchedPlayerId}.");
                        ResetEnrollBuffers();
                        return;
                    }
                }
                _tempTemplates[_enrollStep] = new byte[rawTemplate.Length];
                Array.Copy(rawTemplate, _tempTemplates[_enrollStep], rawTemplate.Length);

                _enrollStep++;
                OnEnrollStepCaptured?.Invoke(_enrollStep);

                if (_enrollStep == 3)
                {
                    byte[] masterTemplate = new byte[2048];
                    int masterSize = masterTemplate.Length;

                    int ret = zkfp2.DBMerge(DbHandle, _tempTemplates[0], _tempTemplates[1], _tempTemplates[2], masterTemplate, ref masterSize);

                    if (ret == zkfp.ZKFP_ERR_OK)
                    {
                        byte[] actualMaster = new byte[masterSize];
                        Array.Copy(masterTemplate, actualMaster, masterSize);

                        if (EnrollmentQueue != null)
                        {
                            OnMasterTemplateGenerated?.Invoke(actualMaster, EnrollmentQueue);
                        }
                    }
                    else
                    {
                        OnEnrollFailed?.Invoke($"فشل دمج البصمات الثلاث (كود الخطأ: {ret}). يرجى التأكد من وضع الإصبع بثبات وبنفس الزاوية.");
                    }

                    ResetEnrollBuffers();
                }
            }
            catch (Exception ex)
            {
                OnEnrollFailed?.Invoke($"خطأ تقني أثناء معالجة الخطوات: {ex.Message}");
                ResetEnrollBuffers();
            }
        }
        private int CheckIfFingerprintExists(byte[] template)
        {
            if (DbHandle == IntPtr.Zero) return 0;

            int score = 0;
            int matchedPlayerId = 0;

            int ret = zkfp2.DBIdentify(DbHandle, template, ref matchedPlayerId, ref score);

            return (ret == zkfp.ZKFP_ERR_OK) ? matchedPlayerId : 0;
        }
        private void ResetEnrollBuffers()
        {
            _enrollStep = 0;
            _tempTemplates[0] = null!;
            _tempTemplates[1] = null!;
            _tempTemplates[2] = null!;
        }
    }

    public enum SystemState
    {
        Verification,
        Enrollment
    }
}
