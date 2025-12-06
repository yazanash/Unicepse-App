using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;              
using System.Windows.Controls;     
using System.Windows.Documents;   
using System.Windows.Markup;
using System.Windows.Media;
using Uniceps.utlis.Pages;
using Uniceps.ViewModels.PrintRoutineViewModel;

namespace Uniceps.Helpers.Mappers
{
    public static class BuildRoutineDocument
    {
        public static FixedDocument BuildDocument(PlayerRoutinePrintViewModel vm)
        {
            var doc = new FixedDocument();
            var fullPage = new PlayerRoutinePrintView { DataContext = vm };
            fullPage.UpdateLayout();

            fullPage.Measure(new Size(793, double.PositiveInfinity));
            fullPage.Arrange(new Rect(0, 0, 793, fullPage.DesiredSize.Height));
            fullPage.UpdateLayout();

            double fullHeight = fullPage.DesiredSize.Height;

            var fixedPage = new FixedPage
            {
                Width = 793,
                Height = fullHeight
            };

            var vb = new VisualBrush(fullPage)
            {
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top,
                TileMode = TileMode.None,
                Viewbox = new Rect(0, 0, 793, fullHeight),
                ViewboxUnits = BrushMappingMode.Absolute
            };

            var rectangle = new System.Windows.Shapes.Rectangle
            {
                Width = 793,
                Height = fullHeight,
                Fill = vb
            };

            fixedPage.Children.Add(rectangle);
            var pc = new PageContent();
            ((IAddChild)pc).AddChild(fixedPage);
            doc.Pages.Add(pc);

            return doc;
        }
    }
}
