using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlatinumGymPro.utlis.Components
{
    /// <summary>
    /// Interaction logic for MetricItem.xaml
    /// </summary>
    public partial class MetricItem : UserControl
    {
        public MetricItem()
        {
            InitializeComponent();
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            ("Title", typeof(string), typeof(MetricItem));

        public double MetricValue
        {
            get { return (double)GetValue(MetricValueProperty); }
            set { SetValue(MetricValueProperty, value); }
        }

        public static readonly DependencyProperty MetricValueProperty = DependencyProperty.Register
            ("MetricValue", typeof(double), typeof(MetricItem));

        public Uri Icon
        {
            get { return (Uri)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register
            ("Icon", typeof(Uri), typeof(MetricItem));
    }
}
