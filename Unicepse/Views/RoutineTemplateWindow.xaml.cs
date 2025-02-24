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
using System.Windows.Shapes;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.ViewModels;
using Unicepse.ViewModels.RoutineViewModels;
namespace Unicepse.Views
{
    /// <summary>
    /// Interaction logic for RoutineTemplateWindow.xaml
    /// </summary>
    public partial class RoutineTemplateWindow : Window
    {
        public RoutineTemplateWindow()
        {
            InitializeComponent();

            // Add a few initial daily lists
            //AddDailyList("Monday");
            //AddDailyList("Tuesday");
           

        }
        
        //private void ListBoxItem_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        ListBoxItem listBoxItem = sender as ListBoxItem;
        //        if (listBoxItem != null)
        //        {
        //            DragDrop.DoDragDrop(listBoxItem, listBoxItem.Content, DragDropEffects.Copy);
        //        }
        //    }
        //}

        //private void AddDailyList(string day)
        //{
        //    ListBox listBox = new ListBox
        //    {
        //        Width = 200,
        //        Height = 300,
        //        AllowDrop = true,
        //        Margin = new Thickness(5)
        //    };

        //    listBox.Drop += ListBox_Drop;
        //    listBox.Tag = day; // Store the day in the Tag property

        //    // Add header for the day
        //    StackPanel stackPanel = new StackPanel();
        //    stackPanel.Children.Add(new TextBlock { Text = day, FontWeight = FontWeights.Bold, Margin = new Thickness(5) });
        //    stackPanel.Children.Add(listBox);

        //    DailyListsPanel.Children.Add(stackPanel);
        //}
        //private void ListBox_Drop(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.StringFormat))
        //    {
        //        string exercise = e.Data.GetData(DataFormats.StringFormat).ToString();
        //        (sender as ListBox).Items.Add(exercise);
        //    }
        //}
        private void ListBoxItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var listViewItem = sender as ListViewItem;
                if (listViewItem != null)
                {
                    var exercise = listViewItem.DataContext as ExercisesListItemViewModel;
                    if (exercise != null)
                    {
                        DragDrop.DoDragDrop(listViewItem, exercise, DragDropEffects.Move);
                    }
                }
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ExercisesListItemViewModel)))
            {
                var exercise = e.Data.GetData(typeof(ExercisesListItemViewModel)) as ExercisesListItemViewModel;
                ListView listView = sender as ListView;
                if (exercise != null && listView != null && listView.ItemsSource is ObservableCollection<RoutineItemFillViewModel> exercises)
                {
                    RoutineItems routineItems = new();
                    routineItems.Exercises = exercise.Exercises;
                    exercises.Add(new RoutineItemFillViewModel(routineItems));
                }
            }
        }
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    AddDailyList("Monday");
        //}
    }
}
