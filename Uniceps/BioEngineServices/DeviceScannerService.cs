using libzkfpcsharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uniceps.BioEngineServices
{
    public class DeviceScannerService
    {
        private IntPtr _devHandle = IntPtr.Zero;
        private int _width;
        private int _height;
        private CancellationTokenSource? _cts;
        private readonly object _lockObj = new object();

        public bool IsConnected { get; private set; } = false;

        public event Action<byte[]>? OnFingerprintCaptured;
        public event Action? OnDeviceConnected;
        public event Action? OnDeviceDisconnected;

        public void Start()
        {
            if (_cts != null) return;
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            Task.Run(async () =>
            {
                byte[]? fpBuffer = null;
                byte[] capResult = new byte[2048];

                while (!token.IsCancellationRequested)
                {
                    if (!IsConnected)
                    {
                        if (TryConnect())
                        {
                            lock (_lockObj)
                            {
                                fpBuffer = new byte[_width * _height];
                            }
                            OnDeviceConnected?.Invoke();
                        }
                        else
                        {
                            await Task.Delay(3000, token);
                            continue;
                        }
                    }

                    int ret = zkfp.ZKFP_ERR_OK;
                    int cbCapResult = capResult.Length;

                    lock (_lockObj)
                    {
                        if (_devHandle != IntPtr.Zero && fpBuffer != null)
                        {
                            ret = zkfp2.AcquireFingerprint(_devHandle, fpBuffer, capResult, ref cbCapResult);
                        }
                        else
                        {
                            ret = -1;
                        }
                    }

                    if (ret == zkfp.ZKFP_ERR_OK)
                    {
                        byte[] actualTemplate = new byte[cbCapResult];
                        Array.Copy(capResult, actualTemplate, cbCapResult);

                        OnFingerprintCaptured?.Invoke(actualTemplate);

                        await Task.Delay(500, token);
                    }
                    else
                    {
                        // فحص انقطاع الاتصال بطلب آمن
                        if (IsConnected && zkfp2.GetDeviceCount() == 0)
                        {
                            ResetConnection();
                            OnDeviceDisconnected?.Invoke();
                        }

                        await Task.Delay(100, token);
                    }
                }
            }, token);
        }

        private bool TryConnect()
        {
            lock (_lockObj)
            {
                try
                {
                    // Init تُستدعى مرة واحدة فقط عند الحاجة، وإذا نجحت لا تعاد تهيئتها
                    int initRet = zkfp2.Init();
                    if (initRet != zkfp.ZKFP_ERR_OK && initRet != 1) return false; // 1 تعني أنها مهيأة بالفعل

                    if (zkfp2.GetDeviceCount() == 0) return false;

                    _devHandle = zkfp2.OpenDevice(0);
                    if (_devHandle == IntPtr.Zero) return false;

                    byte[] param = new byte[4];
                    int size = 4;

                    zkfp2.GetParameters(_devHandle, 1, param, ref size);
                    _width = BitConverter.ToInt32(param, 0);

                    zkfp2.GetParameters(_devHandle, 2, param, ref size);
                    _height = BitConverter.ToInt32(param, 0);

                    IsConnected = true;
                    return true;
                }
                catch
                {
                    ResetConnection();
                    return false;
                }
            }
        }

        private void ResetConnection()
        {
            lock (_lockObj)
            {
                IsConnected = false;
                if (_devHandle != IntPtr.Zero)
                {
                    zkfp2.CloseDevice(_devHandle);
                    _devHandle = IntPtr.Zero;
                }
                // يفضل عدم استدعاء zkfp2.Terminate() هنا لكي لا تُدمر المكتبة إذا كان هناك محاولات إعادة اتصال مستمرة
            }
        }

        public void Stop()
        {
            _cts?.Cancel();
            _cts = null;

            ResetConnection();

            lock (_lockObj)
            {
                zkfp2.Terminate(); // الترديد النهائي للمكتبة عند إيقاف الخدمة كلياً
            }
        }
    }
}
