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

        public static readonly DependencyProperty DayGroupItemDropCommandProperty =
           DependencyProperty.Register("DayGroupItemDropCommand", typeof(ICommand), typeof(DayGroupListView),
               new PropertyMetadata(null));

        public static readonly DependencyProperty IncomingDayGroupItemProperty =
           DependencyProperty.Register("IncomingDayGroupItem", typeof(object), typeof(DayGroupListView),
               new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object IncomingDayGroupItem
        {
            get { return (object)GetValue(IncomingDayGroupItemProperty); }
            set { SetValue(IncomingDayGroupItemProperty, value); }
        }
        public ICommand DayGroupItemDropCommand
        {
            get { return (ICommand)GetValue(DayGroupItemDropCommandProperty); }
            set { SetValue(DayGroupItemDropCommandProperty, value); }
        }
        public static readonly DependencyProperty DayGroupItemInsertedCommandProperty =
          DependencyProperty.Register("DayGroupItemInsertedCommand", typeof(ICommand), typeof(DayGroupListView),
              new PropertyMetadata(null));

        public ICommand DayGroupItemInsertedCommand
        {
            get { return (ICommand)GetValue(DayGroupItemInsertedCommandProperty); }
            set { SetValue(DayGroupItemInsertedCommandProperty, value); }
        }

        public static readonly DependencyProperty InsertedDayGroupItemProperty =
            DependencyProperty.Register("InsertedDayGroupItem", typeof(object), typeof(DayGroupListView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object InsertedDayGroupItem
        {
            get { return (object)GetValue(InsertedDayGroupItemProperty); }
            set { SetValue(InsertedDayGroupItemProperty, value); }
        }

        public static readonly DependencyProperty TargetDayGroupItemProperty =
            DependencyProperty.Register("TargetDayGroupItem", typeof(object), typeof(DayGroupListView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object TargetDayGroupItem
        {
            get { return (object)GetValue(TargetDayGroupItemProperty); }
            set { SetValue(TargetDayGroupItemProperty, value); }
        }

        public static readonly DependencyProperty RemovedDayGroupItemProperty =
          DependencyProperty.Register("RemovedDayGroupItem", typeof(object), typeof(DayGroupListView),
              new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object RemovedDayGroupItem
        {
            get { return (object)GetValue(RemovedDayGroupItemProperty); }
            set { SetValue(RemovedDayGroupItemProperty, value); }
        }

        public static readonly DependencyProperty DayGroupRemovedCommandProperty =
          DependencyProperty.Register("DayGroupItemRemovedCommand", typeof(ICommand), typeof(DayGroupListView),
              new PropertyMetadata(null));

        public ICommand DayGroupItemRemovedCommand
        {
            get { return (ICommand)GetValue(DayGroupRemovedCommandProperty); }
            set { SetValue(DayGroupRemovedCommandProperty, value); }
        }



        public static readonly DependencyProperty SaveNewOrderCommandProperty =
         DependencyProperty.Register("SaveNewOrderCommand", typeof(ICommand), typeof(DayGroupListView),
             new PropertyMetadata(null));

        public ICommand SaveNewOrderCommand
        {
            get { return (ICommand)GetValue(SaveNewOrderCommandProperty); }
            set { SetValue(SaveNewOrderCommandProperty, value); }
        }
        public DayGroupListView()
        {
            InitializeComponent();
        }
        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
               sender is FrameworkElement frameworkElement)
            {
                object dayGroupItem = frameworkElement.DataContext;
               
                
                DragDropEffects dragDropResult = DragDrop.DoDragDrop(frameworkElement,
                    new DataObject(DataFormats.Serializable, dayGroupItem),
                    DragDropEffects.Move);

                if (dragDropResult == DragDropEffects.None)
                {
                    AddDayGroupItem(dayGroupItem);
                }  
            }
        }
        private void AddDayGroupItem(object todoItem)
        {
            
            if (DayGroupItemDropCommand?.CanExecute(null) ?? false)
            {
                IncomingDayGroupItem = todoItem;
                DayGroupItemDropCommand?.Execute(null);
               
            }
        }
        private void DragOverHandler(object sender, DragEventArgs e)
        {
            if (DayGroupItemInsertedCommand?.CanExecute(null) ?? false)
            {
                if (sender is FrameworkElement element)
                {
                    TargetDayGroupItem = element.DataContext;
                    InsertedDayGroupItem = e.Data.GetData(DataFormats.Serializable);

                    DayGroupItemInsertedCommand?.Execute(null);
                }
            }
        }
        private void TodoItemList_DragLeave(object sender, DragEventArgs e)
        {
            HitTestResult result = VisualTreeHelper.HitTest(lvItems, e.GetPosition(lvItems));

            if (result == null)
            {
                if (DayGroupItemRemovedCommand?.CanExecute(null) ?? false)
                {
                    RemovedDayGroupItem = e.Data.GetData(DataFormats.Serializable);
                    DayGroupItemRemovedCommand?.Execute(null);
                }
            }
        }
        private void TodoItemList_DragOver(object sender, DragEventArgs e)
        {
            object todoItem = e.Data.GetData(DataFormats.Serializable);

            AddDayGroupItem(todoItem);
        }

        private void ListBoxItem_Drop(object sender, DragEventArgs e)
        {
            if (SaveNewOrderCommand?.CanExecute(null) ?? false)
            {
                SaveNewOrderCommand?.Execute(null);
            }
        }
    }
}
