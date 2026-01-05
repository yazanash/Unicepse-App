using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Core.Models.Employee;

namespace Uniceps.Services
{
    public class TrainerDuesExcelService
    {
        private readonly Dictionary<string, int> columnOrder = new()
        {
            ["اسم اللاعب"] = 1,
            ["الرياضة"] = 2,
            ["قيمة الدفعة"] = 3,
            ["من تاريخ"] = 4,
            ["إلى تاريخ"] = 5,
            ["المستحق الشهري"] = 6,
            ["حالة الدفع"] = 7,
        };
        public void ExportToExcel(string filePath, TrainerDueses data)
        {
            try
            {
                using var workbook = new XLWorkbook();

                string sheetName = data.Trainer?.FullName ?? "مدرب";
                var sheet = workbook.Worksheets.Add(sheetName);
                sheet.RightToLeft = true;

                WriteSummaryHeader(sheet, data);

                int tableHeaderRow = 10;
                foreach (var kvp in columnOrder)
                {
                    var cell = sheet.Cell(tableHeaderRow, kvp.Value);
                    cell.Value = kvp.Key;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                }

                int rowIndex = tableHeaderRow + 1;
                foreach (var detail in data.Details)
                {
                    sheet.Cell(rowIndex, 1).Value = detail.PlayerName;
                    sheet.Cell(rowIndex, 2).Value = detail.SportName;
                    sheet.Cell(rowIndex, 3).Value = detail.PaymentValue;
                    sheet.Cell(rowIndex, 4).Value = detail.CoveredFrom.ToShortDateString();
                    sheet.Cell(rowIndex, 5).Value = detail.CoveredTo.ToShortDateString();
                    sheet.Cell(rowIndex, 6).Value = Math.Round(detail.AmountForMonth, 1);
                    rowIndex++;
                }

                sheet.Columns().AdjustToContents(); // تنسيق عرض الأعمدة تلقائياً
                workbook.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ في تصدير ملف الاكسل الخاص بالمستحقات: " + ex.Message);
            }
        }
        private void WriteSummaryHeader(IXLWorksheet sheet, TrainerDueses dues)
        {
            // تنسيق بسيط للملخص في أعلى الصفحة
            sheet.Cell(1, 1).Value = "كشف مستحقات المدرب:";
            sheet.Cell(1, 2).Value = dues.Trainer?.FullName;
            sheet.Cell(1, 1).Style.Font.Bold = true;

            sheet.Cell(2, 1).Value = "تاريخ الإصدار:";
            sheet.Cell(2, 2).Value = DateTime.Now.ToShortDateString();

            sheet.Cell(3, 1).Value = "إجمالي الاشتراكات:";
            sheet.Cell(3, 2).Value = dues.TotalSubscriptions;

            sheet.Cell(4, 1).Value = "النسبة:";
            sheet.Cell(4, 2).Value = $"{dues.Parcent * 100}%";

            sheet.Cell(5, 1).Value = " المستحقات (راتب + نسبة):";
            sheet.Cell(5, 2).Value = dues.Salary + dues.TotalSubscriptions; // فرضية حسابية
            sheet.Cell(5, 2).Style.Font.FontColor = XLColor.Green;

            sheet.Cell(6, 1).Value = " المسحوبات :";
            sheet.Cell(6, 2).Value = dues.Credits; // فرضية حسابية
            sheet.Cell(6, 2).Style.Font.FontColor = XLColor.Red;
            double income = dues.Salary + dues.TotalSubscriptions - dues.Credits;
            sheet.Cell(7, 1).Value = " الصافي :";
            sheet.Cell(7, 2).Value = income; // فرضية حسابية
            sheet.Cell(7, 2).Style.Font.FontColor = income > 0 ? XLColor.Green : XLColor.Red;

        }
        public List<TrainerDueses> ImportFromExcel(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
