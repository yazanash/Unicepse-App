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

namespace Unicepse.Views.RoutineViews
{
    /// <summary>
    /// Interaction logic for EditRoutineView.xaml
    /// </summary>
    public partial class EditRoutineView : UserControl
    {
        public EditRoutineView()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ex_list.Items.Count > 0)
            {
                ex_list.ScrollIntoView(ex_list.Items[0]);
            }
            if (it_list.Items.Count > 0)
            {
                it_list.ScrollIntoView(it_list.Items[0]);
            }
        }
    }
}
