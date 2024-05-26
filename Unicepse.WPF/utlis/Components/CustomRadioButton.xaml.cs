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
using MahApps.Metro.IconPacks;

namespace Unicepse.WPF.utlis.Components
{
    /// <summary>
    /// Interaction logic for CustomRadioButton.xaml
    /// </summary>
    public partial class CustomRadioButton : UserControl
    {
        public CustomRadioButton()
        {
            InitializeComponent();
        }

        public PackIconMaterialKind Icon
        {
            get { return (PackIconMaterialKind)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register
            ("Icon", typeof(PackIconMaterialKind), typeof(CustomRadioButton));


        public string RadioText
        {
            get { return (string)GetValue(RadioTextProperty); }
            set { SetValue(RadioTextProperty, value); }
        }

        public static readonly DependencyProperty RadioTextProperty = DependencyProperty.Register
            ("RadioText", typeof(string), typeof(CustomRadioButton));
    }
}
