using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Uniceps.utlis.Effects;
using Uniceps.ViewModels.RoutineTemplateViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;

namespace Uniceps.Views.RoutineTemplateViews.RoutineComponent
{
    /// <summary>
    /// Interaction logic for DayGroupListView.xaml
    /// </summary>
    public partial class DayGroupListView : UserControl
    {

       
        public DayGroupListView()
        {
            InitializeComponent();
        }

        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void MyPopup_Opened(object sender, EventArgs e)
        {
            var popup = sender as Popup;
            if (popup == null)
                return;
            // Get the root visual of the Popup
            var root = popup.Child;
            if (root == null) return;

            // Search for the TextBox inside
            var textBox = FindChild<TextBox>(root);
            if (textBox == null) return;

            // Focus the TextBox
            textBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBox.Focus();
                textBox.SelectAll();
            }));
        }
        public static T? FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild)
                    return typedChild;

                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
