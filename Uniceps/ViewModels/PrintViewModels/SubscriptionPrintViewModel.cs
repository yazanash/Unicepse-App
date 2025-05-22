using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Uniceps.utlis.common;
using Uniceps.Stores;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.ViewModels.PrintViewModels
{
    public class SubscriptionPrintViewModel : ViewModelBase
    {
        public Subscription Subscription;
        private readonly ObservableCollection<PaymentListItemViewModel> _paymentListItemViewModels;
        private readonly LicenseDataStore _licenseDataStore;
        public IEnumerable<PaymentListItemViewModel> PaymentsList => _paymentListItemViewModels;
        public SubscriptionPrintViewModel(Subscription subscription, LicenseDataStore licenseDataStore)
        {
            Subscription = subscription;
            _paymentListItemViewModels = new ObservableCollection<PaymentListItemViewModel>();
            foreach (var pay in Subscription.Payments!)
            {
                PaymentListItemViewModel paymentListItemViewModel = new PaymentListItemViewModel(pay);
                _paymentListItemViewModels.Add(paymentListItemViewModel);
            }
            _licenseDataStore = licenseDataStore;
            if (_licenseDataStore.CurrentGymProfile != null)
            {
                GymName = _licenseDataStore.CurrentGymProfile!.GymName;
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(_licenseDataStore.CurrentGymProfile!.Logo!);
                    bitmap.EndInit();
                    GymLogo = bitmap;
                }
                catch
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("pack://application:,,,/Resources/Assets/logo.png");
                    bitmap.EndInit();
                    GymLogo = bitmap;
                }

            }
            else
            {

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/Assets/logo.png");
                bitmap.EndInit();
                GymLogo = bitmap;

            }

        }
        private string? _gymName;
        public string? GymName
        {
            get { return _gymName; }
            set { _gymName = value; OnPropertyChanged(nameof(GymName)); }
        }
        private BitmapImage? _gymLogo;

        public BitmapImage? GymLogo
        {
            get { return _gymLogo; }
            set { _gymLogo = value; OnPropertyChanged(nameof(GymLogo)); }
        }
        public int Id => Subscription.Id;
        public string? PlayerName => Subscription.Player!.FullName;
        public double PlayerBalance => Subscription.Player!.Balance;
        public string? SportName => Subscription.Sport!.Name;
        public int SubDays => Subscription.DaysCount;
        public DateTime LastCheck => Subscription.LastCheck;
        public string? Trainer => Subscription.Trainer != null ? Subscription.Trainer!.FullName : "بدون مدرب";
        public string RollDate => Subscription.RollDate.ToString("ddd,MMM dd,yyy");
        public double Price => Subscription.Price;
        public double OfferValue => Subscription.OfferValue;
        public string? OfferDes => Subscription.OfferDes;
        public double PriceAfterOffer => Subscription.PriceAfterOffer;
        public string IsPrivate => Subscription.IsPrivate ? "تدريب خاص" : "لا يوجد";
        public double PrivatePrice => Subscription.IsPrivate ? Subscription.PrivatePrice : 0;
        public double PaidValue => Subscription.PaidValue;
        public string EndDate => Subscription.EndDate.ToString("ddd,MMM dd,yyy");
    }
}
