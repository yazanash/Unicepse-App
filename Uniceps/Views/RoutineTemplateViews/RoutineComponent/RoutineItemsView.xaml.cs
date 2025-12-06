using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;

namespace Uniceps.Views.RoutineTemplateViews.RoutineComponent
{
    /// <summary>
    /// Interaction logic for RoutineItemsView.xaml
    /// </summary>
    public partial class RoutineItemsView : UserControl
    {
        public RoutineItemsView()
        {
            InitializeComponent();
        }

        private void PlayerDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (PlayerDataGrid.SelectedItem != null)
            {
                var item = (DataGridRow)PlayerDataGrid.ItemContainerGenerator.ContainerFromItem(PlayerDataGrid.SelectedItem);
                item?.Focus();  // إعادة التركيز
            }
        }
    }
}
