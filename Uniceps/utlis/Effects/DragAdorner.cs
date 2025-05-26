using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace Uniceps.utlis.Effects
{
    internal class DragAdorner : Adorner
    {
        private readonly VisualBrush _brush;

        public DragAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _brush = new VisualBrush(adornedElement);
            _brush.Opacity = 0.7;  // تأثير شفافية
        }

        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(_brush, null, new Rect(new Point(0, 0), AdornedElement.RenderSize));
        }
    }
}
