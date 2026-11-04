using libzkfpcsharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;

namespace Uniceps.BioEngineServices
{
    public class BiometricStorageService
    {
        private IntPtr _dbHandle = IntPtr.Zero;
        public IntPtr GetDbHandle() => _dbHandle;
        public BiometricStorageService()
        {
            _dbHandle = zkfp2.DBInit();
            if (_dbHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("فشل تهيئة الذاكرة اللحظية (RAM) الخاصة بالـ SDK.");
            }
        }
        public  void LoadFingerprintsIntoRam(List<Player> users)
        {
            if (_dbHandle == IntPtr.Zero) return;

            zkfp2.DBClear(_dbHandle);

            foreach (var user in users)
            {
                if (string.IsNullOrEmpty(user.FingerprintData)) continue;
                byte[] blob = zkfp2.Base64ToBlob(user.FingerprintData);
                zkfp2.DBAdd(_dbHandle, user.Id, blob);
            }
        }
        public bool AddSingleUserToRam(byte[] masterTemplate, Player queue)
        {
            if (_dbHandle == IntPtr.Zero) return false;

            try
            {
                string base64Template = zkfp2.BlobToBase64(masterTemplate, masterTemplate.Length);
                zkfp2.DBAdd(_dbHandle, queue.Id, masterTemplate);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int Verify(byte[] capturedTemplate, ref int score)
        {
            if (_dbHandle == IntPtr.Zero) return -1;

            int registeredPlayerId = 0;

            int ret = zkfp2.DBIdentify(_dbHandle, capturedTemplate, ref registeredPlayerId, ref score);

            if (ret == zkfp.ZKFP_ERR_OK && score >= 80)
            {
                return registeredPlayerId; 
            }

            return -1;
        }

        public void Close()
        {
            if (_dbHandle != IntPtr.Zero)
            {
                zkfp2.DBFree(_dbHandle);
                _dbHandle = IntPtr.Zero;
            }
        }
    }
}
