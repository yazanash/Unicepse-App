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

namespace Uniceps.Views.RoutineTemplateViews.RoutineComponent
{
    /// <summary>
    /// Interaction logic for ExercisesListView.xaml
    /// </summary>
    public partial class ExercisesListView : UserControl
    {
        public static readonly DependencyProperty ExerciseSelectedCommandProperty =
         DependencyProperty.Register("ExerciseSelectedCommand", typeof(ICommand), typeof(ExercisesListView),
             new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
           DependencyProperty.Register("SelectedItem", typeof(object), typeof(ExercisesListView),
               new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public ICommand ExerciseSelectedCommand
        {
            get { return (ICommand)GetValue(ExerciseSelectedCommandProperty); }
            set { SetValue(ExerciseSelectedCommandProperty, value); }
        }


        public static readonly DependencyProperty ExerciseUnSelectedCommandProperty =
          DependencyProperty.Register("ExerciseUnSelectedCommand", typeof(ICommand), typeof(ExercisesListView),
              new PropertyMetadata(null));

        public ICommand ExerciseUnSelectedCommand
        {
            get { return (ICommand)GetValue(ExerciseUnSelectedCommandProperty); }
            set { SetValue(ExerciseUnSelectedCommandProperty, value); }
        }
        public static readonly DependencyProperty UnSelectedItemProperty =
          DependencyProperty.Register("UnSelectedItem", typeof(object), typeof(ExercisesListView),
              new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object UnSelectedItem
        {
            get { return (object)GetValue(UnSelectedItemProperty); }
            set { SetValue(UnSelectedItemProperty, value); }
        }
        public ExercisesListView()
        {
            InitializeComponent();
        }
        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            if (ExerciseSelectedCommand?.CanExecute(null) ?? false)
            {
                if (sender is FrameworkElement element)
                {
                    SelectedItem = element.DataContext;
                    ExerciseSelectedCommand?.Execute(null);
                }
            }
        }

        private void ListBoxItem_Unselected(object sender, RoutedEventArgs e)
        {
            if (ExerciseUnSelectedCommand?.CanExecute(null) ?? false)
            {
                if (sender is FrameworkElement element)
                {
                    UnSelectedItem = element.DataContext;
                    ExerciseUnSelectedCommand?.Execute(null);
                }
            }
        }
    }
}
