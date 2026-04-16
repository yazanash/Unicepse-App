using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Uniceps.Stores;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.Core.Models.Subscription;
using System.IO;
using Uniceps.SystemServices;
using QRCoder;

namespace Uniceps.ViewModels.PrintViewModels
{
    public class SubscriptionPrintViewModel : ViewModelBase
    {
        public Subscription Subscription;
        private readonly ObservableCollection<PaymentListItemViewModel> _paymentListItemViewModels;
        public IEnumerable<PaymentListItemViewModel> PaymentsList => _paymentListItemViewModels;
        public SubscriptionPrintViewModel(Subscription subscription)
        {
            Subscription = subscription;

            _paymentListItemViewModels = new ObservableCollection<PaymentListItemViewModel>();

            foreach (var item in Subscription.Payments!)
            {
                _paymentListItemViewModels.Add(new PaymentListItemViewModel(item));
            }
            GymName = SettingsManager.Current.GymName;
            GymPhone = SettingsManager.Current.ContactNumber;
            GymOwner = SettingsManager.Current.OwnerName;
            LoadProfileImage(SettingsManager.Current.LogoPath);
            QRCodeImage = GenerateQRCode(subscription.Code??"");
        }
        private void LoadProfileImage(string? localPath)
        {
            if (!File.Exists(localPath))
                return;

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(localPath);
            bitmap.EndInit();
            bitmap.Freeze();
            GymLogo = bitmap;
        }
        private string? _gymName;
        public string? GymName
        {
            get { return _gymName; }
            set { _gymName = value; OnPropertyChanged(nameof(GymName)); }
        }
        private string? _gymPhone;
        public string? GymPhone
        {
            get { return _gymPhone; }
            set { _gymPhone = value; OnPropertyChanged(nameof(GymPhone)); }
        }
        private string? _gymOwner;
        public string? GymOwner
        {
            get { return _gymOwner; }
            set { _gymOwner = value; OnPropertyChanged(nameof(GymOwner)); }
        }
        private BitmapImage? _gymLogo;

        public BitmapImage? GymLogo
        {
            get { return _gymLogo; }
            set { _gymLogo = value; OnPropertyChanged(nameof(GymLogo)); }
        }
        public int Id => Subscription.Id;
        public string? PlayerName => Subscription.PlayerName;
        public string? Code => Subscription.Code;
        public string? SportName => Subscription.SportName;
        public int SubDays => Subscription.DaysCount;
        public DateTime LastCheck => Subscription.LastCheck;
        public string? Trainer =>  Subscription.TrainerName?? "بدون مدرب";
        public string RollDate => Subscription.RollDate.ToString("yyyy/MM/dd");
        public double Price => Subscription.Price;
        public double OfferValue => Subscription.OfferValue;
        public string? OfferDes => Subscription.OfferDes;
        public double PriceAfterOffer => Subscription.PriceAfterOffer;
        public string EndDate => Subscription.EndDate.ToString("yyyy/MM/dd");
        private BitmapImage? _qrCodeImage;
        public BitmapImage? QRCodeImage
        {
            get => _qrCodeImage;
            set { _qrCodeImage = value; OnPropertyChanged(nameof(QRCodeImage)); }
        }
        public BitmapImage GenerateQRCode(string text)
        {
            // 1. إنشاء الـ QR Generator
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                // 2. توليد الصورة كـ Array من البايتات
                byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);

                // 3. تحويل الـ Byte Array إلى BitmapImage ليعرضه WPF
                using (MemoryStream ms = new MemoryStream(qrCodeAsPngByteArr))
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = ms;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();
                    return bi;
                }
            }
        }
    }
}
