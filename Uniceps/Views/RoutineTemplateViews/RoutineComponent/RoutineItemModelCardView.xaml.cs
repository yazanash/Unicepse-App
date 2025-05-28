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
    /// Interaction logic for RoutineItemModelCardView.xaml
    /// </summary>
    public partial class RoutineItemModelCardView : UserControl
    {
        public static readonly DependencyProperty SetModelItemDropCommandProperty =
          DependencyProperty.Register("SetModelItemDropCommand", typeof(ICommand), typeof(RoutineItemModelCardView),
              new PropertyMetadata(null));

        public static readonly DependencyProperty IncomingSetModelItemProperty =
           DependencyProperty.Register("IncomingSetModelItem", typeof(object), typeof(RoutineItemModelCardView),
               new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object IncomingSetModelItem
        {
            get { return (object)GetValue(IncomingSetModelItemProperty); }
            set { SetValue(IncomingSetModelItemProperty, value); }
        }
        public ICommand SetModelItemDropCommand
        {
            get { return (ICommand)GetValue(SetModelItemDropCommandProperty); }
            set { SetValue(SetModelItemDropCommandProperty, value); }
        }
        public static readonly DependencyProperty SetModelItemInsertedCommandProperty =
          DependencyProperty.Register("SetModelItemInsertedCommand", typeof(ICommand), typeof(RoutineItemModelCardView),
              new PropertyMetadata(null));

        public ICommand SetModelItemInsertedCommand
        {
            get { return (ICommand)GetValue(SetModelItemInsertedCommandProperty); }
            set { SetValue(SetModelItemInsertedCommandProperty, value); }
        }

        public static readonly DependencyProperty InsertedSetModelItemProperty =
            DependencyProperty.Register("InsertedSetModelItem", typeof(object), typeof(RoutineItemModelCardView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object InsertedSetModelItem
        {
            get { return (object)GetValue(InsertedSetModelItemProperty); }
            set { SetValue(InsertedSetModelItemProperty, value); }
        }

        public static readonly DependencyProperty TargetRoutineItemProperty =
            DependencyProperty.Register("TargetSetModelItem", typeof(object), typeof(RoutineItemModelCardView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object TargetSetModelItem
        {
            get { return (object)GetValue(TargetRoutineItemProperty); }
            set { SetValue(TargetRoutineItemProperty, value); }
        }

        public static readonly DependencyProperty RemovedSetModelItemProperty =
          DependencyProperty.Register("RemovedSetModelItem", typeof(object), typeof(RoutineItemModelCardView),
              new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object RemovedSetModelItem
        {
            get { return (object)GetValue(RemovedSetModelItemProperty); }
            set { SetValue(RemovedSetModelItemProperty, value); }
        }

        public static readonly DependencyProperty SetModelItemRemovedCommandProperty =
          DependencyProperty.Register("SetModelItemRemovedCommand", typeof(ICommand), typeof(RoutineItemModelCardView),
              new PropertyMetadata(null));

        public ICommand SetModelItemRemovedCommand
        {
            get { return (ICommand)GetValue(SetModelItemRemovedCommandProperty); }
            set { SetValue(SetModelItemRemovedCommandProperty, value); }
        }



        public static readonly DependencyProperty SaveNewSetModelItemOrderCommandProperty =
         DependencyProperty.Register("SaveNewSetModelItemOrderCommand", typeof(ICommand), typeof(RoutineItemModelCardView),
             new PropertyMetadata(null));

        public ICommand SaveNewSetModelItemOrderCommand
        {
            get { return (ICommand)GetValue(SaveNewSetModelItemOrderCommandProperty); }
            set { SetValue(SaveNewSetModelItemOrderCommandProperty, value); }
        }
        public RoutineItemModelCardView()
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

            if (SetModelItemDropCommand?.CanExecute(null) ?? false)
            {
                IncomingSetModelItem = todoItem;
                SetModelItemDropCommand?.Execute(null);

            }
        }
        private void DragOverHandler(object sender, DragEventArgs e)
        {
            if (SetModelItemInsertedCommand?.CanExecute(null) ?? false)
            {
                if (sender is FrameworkElement element)
                {
                    TargetSetModelItem = element.DataContext;
                    InsertedSetModelItem = e.Data.GetData(DataFormats.Serializable);

                    SetModelItemInsertedCommand?.Execute(null);
                }
            }
        }
        private void TodoItemList_DragLeave(object sender, DragEventArgs e)
        {
            HitTestResult result = VisualTreeHelper.HitTest(lvItems, e.GetPosition(lvItems));

            if (result == null)
            {
                if (SetModelItemRemovedCommand?.CanExecute(null) ?? false)
                {
                    RemovedSetModelItem = e.Data.GetData(DataFormats.Serializable);
                    SetModelItemRemovedCommand?.Execute(null);
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
            if (SaveNewSetModelItemOrderCommand?.CanExecute(null) ?? false)
            {
                SaveNewSetModelItemOrderCommand?.Execute(null);
            }
        }
    }
}
