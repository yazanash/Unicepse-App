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
using System.Windows.Shapes;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Views.RoutineTemplateViews.RoutineComponent
{
    /// <summary>
    /// Interaction logic for RoutineItemModelCardWindowView.xaml
    /// </summary>
    public partial class RoutineItemModelCardWindowView : Window
    {
        public RoutineItemModelCardWindowView()
        {
            InitializeComponent();
            this.DataContextChanged += RoutineItemModelCardWindowView_DataContextChanged;
        }

        private void RoutineItemModelCardWindowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as SetModelItemsListViewModel;
            if (vm != null)
                vm.SetsUpdated += () => this.Close();
        }

        private void DayGroupList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool isShortcut =
       (Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.Up) ||
       (Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.Down);

            // إذا ما كانت الاختصار المطلوب → لا تعمل شي وخلي الTextBox يشتغل طبيعي
            if (!isShortcut)
                return;

            // إذا كان الاختصار صحيح → رجّع الفوكس للـ ListViewItem
            if (DayGroupList.SelectedItem != null)
            {
                var item = DayGroupList
                    .ItemContainerGenerator
                    .ContainerFromItem(DayGroupList.SelectedItem) as ListViewItem;

                if (item != null)
                {
                    item.Focus();
                }
            }
        }
    }
}
