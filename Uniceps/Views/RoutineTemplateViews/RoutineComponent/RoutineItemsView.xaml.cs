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
        public static readonly DependencyProperty RoutineItemDropCommandProperty =
          DependencyProperty.Register("RoutineItemDropCommand", typeof(ICommand), typeof(RoutineItemsView),
              new PropertyMetadata(null));

        public static readonly DependencyProperty IncomingRoutineItemProperty =
           DependencyProperty.Register("IncomingRoutineItem", typeof(object), typeof(RoutineItemsView),
               new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object IncomingRoutineItem
        {
            get { return (object)GetValue(IncomingRoutineItemProperty); }
            set { SetValue(IncomingRoutineItemProperty, value); }
        }
        public ICommand RoutineItemDropCommand
        {
            get { return (ICommand)GetValue(RoutineItemDropCommandProperty); }
            set { SetValue(RoutineItemDropCommandProperty, value); }
        }
        public static readonly DependencyProperty RoutineItemInsertedCommandProperty =
          DependencyProperty.Register("RoutineItemInsertedCommand", typeof(ICommand), typeof(RoutineItemsView),
              new PropertyMetadata(null));

        public ICommand RoutineItemInsertedCommand
        {
            get { return (ICommand)GetValue(RoutineItemInsertedCommandProperty); }
            set { SetValue(RoutineItemInsertedCommandProperty, value); }
        }

        public static readonly DependencyProperty InsertedRoutineItemProperty =
            DependencyProperty.Register("InsertedRoutineItem", typeof(object), typeof(RoutineItemsView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object InsertedRoutineItem
        {
            get { return (object)GetValue(InsertedRoutineItemProperty); }
            set { SetValue(InsertedRoutineItemProperty, value); }
        }

        public static readonly DependencyProperty TargetRoutineItemProperty =
            DependencyProperty.Register("TargetRoutineItem", typeof(object), typeof(RoutineItemsView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object TargetRoutineItem
        {
            get { return (object)GetValue(TargetRoutineItemProperty); }
            set { SetValue(TargetRoutineItemProperty, value); }
        }

        public static readonly DependencyProperty RemovedRoutineItemProperty =
          DependencyProperty.Register("RemovedRoutineItem", typeof(object), typeof(RoutineItemsView),
              new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object RemovedRoutineItem
        {
            get { return (object)GetValue(RemovedRoutineItemProperty); }
            set { SetValue(RemovedRoutineItemProperty, value); }
        }

        public static readonly DependencyProperty RoutineItemRemovedCommandProperty =
          DependencyProperty.Register("RoutineItemRemovedCommand", typeof(ICommand), typeof(RoutineItemsView),
              new PropertyMetadata(null));

        public ICommand RoutineItemRemovedCommand
        {
            get { return (ICommand)GetValue(RoutineItemRemovedCommandProperty); }
            set { SetValue(RoutineItemRemovedCommandProperty, value); }
        }



        public static readonly DependencyProperty SaveNewRoutineItemOrderCommandProperty =
         DependencyProperty.Register("SaveNewRoutineItemOrderCommand", typeof(ICommand), typeof(RoutineItemsView),
             new PropertyMetadata(null));

        public ICommand SaveNewRoutineItemOrderCommand
        {
            get { return (ICommand)GetValue(SaveNewRoutineItemOrderCommandProperty); }
            set { SetValue(SaveNewRoutineItemOrderCommandProperty, value); }
        }
        public RoutineItemsView()
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
                    AddRoutineItem(dayGroupItem);
                }
            }
        }
        private void AddRoutineItem(object todoItem)
        {

            if (RoutineItemDropCommand?.CanExecute(null) ?? false)
            {
                IncomingRoutineItem = todoItem;
                RoutineItemDropCommand?.Execute(null);

            }
        }
        private void DragOverHandler(object sender, DragEventArgs e)
        {
            if (RoutineItemInsertedCommand?.CanExecute(null) ?? false)
            {
                if (sender is FrameworkElement element)
                {
                    TargetRoutineItem = element.DataContext;
                    InsertedRoutineItem = e.Data.GetData(DataFormats.Serializable);

                    RoutineItemInsertedCommand?.Execute(null);
                }
            }
        }
        private void TodoItemList_DragLeave(object sender, DragEventArgs e)
        {
            HitTestResult result = VisualTreeHelper.HitTest(PlayerDataGrid, e.GetPosition(PlayerDataGrid));

            if (result == null)
            {
                if (RoutineItemRemovedCommand?.CanExecute(null) ?? false)
                {
                    RemovedRoutineItem = e.Data.GetData(DataFormats.Serializable);
                    RoutineItemRemovedCommand?.Execute(null);
                }
            }
        }
        private void TodoItemList_DragOver(object sender, DragEventArgs e)
        {
            object todoItem = e.Data.GetData(DataFormats.Serializable);

            AddRoutineItem(todoItem);
        }

        private void ListBoxItem_Drop(object sender, DragEventArgs e)
        {
            if (SaveNewRoutineItemOrderCommand?.CanExecute(null) ?? false)
            {
                SaveNewRoutineItemOrderCommand?.Execute(null);
            }
        }
    }
}
