using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;              // فيه Size, Rect, Point
using System.Windows.Controls;     // فيه UserControl, PageContent, FixedPage
using System.Windows.Documents;    // فيه FixedDocument
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
            var pageSize = new Size(793, 1122); // A4
            double pageHeight = pageSize.Height;

            // بناء الصفحة كاملة مرة واحدة
            var fullPage = new PlayerRoutinePrintView { DataContext = vm };

            fullPage.Measure(new Size(pageSize.Width, double.PositiveInfinity));
            fullPage.Arrange(new Rect(0, 0, pageSize.Width, fullPage.DesiredSize.Height));
            fullPage.UpdateLayout();

            double fullHeight = fullPage.ActualHeight;
            if (fullHeight <= 0)
                fullHeight = fullPage.DesiredSize.Height;

            double offset = 0;

            while (offset < fullHeight)
            {
                var fixedPage = new FixedPage
                {
                    Width = pageSize.Width,
                    Height = pageSize.Height
                };

                // Brush لرسم صفحة كاملة
                var vb = new VisualBrush(fullPage)
                {
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Left,
                    AlignmentY = AlignmentY.Top,
                    TileMode = TileMode.None,
                    Viewbox = new Rect(0, offset, pageSize.Width, pageHeight),
                    ViewboxUnits = BrushMappingMode.Absolute
                };

                var rectangle = new System.Windows.Shapes.Rectangle
                {
                    Width = pageSize.Width,
                    Height = pageHeight,
                    Fill = vb
                };

                fixedPage.Children.Add(rectangle);

                var pc = new PageContent();
                ((IAddChild)pc).AddChild(fixedPage);
                doc.Pages.Add(pc);

                offset += pageHeight;
            }


            return doc;
        }
    }
}
