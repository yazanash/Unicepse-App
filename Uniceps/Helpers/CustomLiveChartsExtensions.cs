using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Helpers
{
    public static class CustomLiveChartsExtensions
    {
        public static void AddLiveChartsAppSettings()
        {
            LiveCharts.Configure(config =>
                config
                    .AddSkiaSharp()
                    .HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('أ')) // يبحث عن خط يدعم العربية
                    .UseRightToLeftSettings() // يضبط اتجاه النصوص والـ Tooltips
            );
        }
    }
}
