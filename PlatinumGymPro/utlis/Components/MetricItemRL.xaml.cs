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
    /// Interaction logic for MetricItemRL.xaml
    /// </summary>
    public partial class MetricItemRL : UserControl
    {
        public MetricItemRL()
        {
            InitializeComponent();
        }
        ///////////////////////////////////////////////////////////////
        ///
        /// R I G H T  A R E A
        /// 
        ///////////////////////////////////////////////////////////
        ///
        #region rightarea
        public string RUPTitle
        {
            get { return (string)GetValue(RUPTitleProperty); }
            set { SetValue(RUPTitleProperty, value); }
        }

        public static readonly DependencyProperty RUPTitleProperty = DependencyProperty.Register
            ("RUPTitle", typeof(string), typeof(MetricItemRL));


        public string RDownTitle
        {
            get { return (string)GetValue(RDownTitleProperty); }
            set { SetValue(RDownTitleProperty, value); }
        }

        public static readonly DependencyProperty RDownTitleProperty = DependencyProperty.Register
            ("RDownTitle", typeof(string), typeof(MetricItemRL));

        public double RUPMetricValue
        {
            get { return (double)GetValue(RUPMetricValueProperty); }
            set { SetValue(RUPMetricValueProperty, value); }
        }

        public static readonly DependencyProperty RUPMetricValueProperty = DependencyProperty.Register
            ("RUPMetricValue", typeof(double), typeof(MetricItemRL));
        public double RDownMetricValue
        {
            get { return (double)GetValue(RDownMetricValueProperty); }
            set { SetValue(RDownMetricValueProperty, value); }
        }

        public static readonly DependencyProperty RDownMetricValueProperty = DependencyProperty.Register
            ("RDownMetricValue", typeof(double), typeof(MetricItemRL));
      
        #endregion
        ///////////////////////////////////////////////////////////////
        ///
        /// L E F T  A R E A
        /// 
        ///////////////////////////////////////////////////////////
        #region leftarea
        public string LUPTitle
        {
            get { return (string)GetValue(LUPTitleProperty); }
            set { SetValue(LUPTitleProperty, value); }
        }

        public static readonly DependencyProperty LUPTitleProperty = DependencyProperty.Register
            ("LUPTitle", typeof(string), typeof(MetricItemRL));


        public string LDownTitle
        {
            get { return (string)GetValue(LDownTitleProperty); }
            set { SetValue(LDownTitleProperty, value); }
        }

        public static readonly DependencyProperty LDownTitleProperty = DependencyProperty.Register
            ("LDownTitle", typeof(string), typeof(MetricItemRL));

        public double LUPMetricValue
        {
            get { return (double)GetValue(LUPMetricValueProperty); }
            set { SetValue(LUPMetricValueProperty, value); }
        }

        public static readonly DependencyProperty LUPMetricValueProperty = DependencyProperty.Register
            ("LUPMetricValue", typeof(double), typeof(MetricItemRL));
        public double LDownMetricValue
        {
            get { return (double)GetValue(LDownMetricValueProperty); }
            set { SetValue(LDownMetricValueProperty, value); }
        }

        public static readonly DependencyProperty LDownMetricValueProperty = DependencyProperty.Register
            ("LDownMetricValue", typeof(double), typeof(MetricItemRL));
        public Uri Icon
        {
            get { return (Uri)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register
            ("Icon", typeof(Uri), typeof(MetricItemRL));
        #endregion
    }
}
