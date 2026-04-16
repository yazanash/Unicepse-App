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
                var sheet = workbook.Worksheets.Add("كشف الحساب الإجمالي");
                sheet.RightToLeft = true;

                // 1. العنوان والملخص العلوي
                WriteSummaryHeader(sheet, data);

                // 2. جدول الرواتب (يبدأ من السطر 12)
                int currentRow = 12;
                sheet.Cell(currentRow, 1).Value = "تفاصيل الرواتب المستحقة";
                sheet.Cell(currentRow, 1).Style.Font.Bold = true;
                currentRow++;

                var salaryHeaders = new[] { "الفترة", "الراتب المقطوع", "المستحق الفعلي", "ملاحظات" };
                for (int i = 0; i < salaryHeaders.Length; i++)
                {
                    var cell = sheet.Cell(currentRow, i + 1);
                    cell.Value = salaryHeaders[i];
                    cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                    cell.Style.Font.Bold = true;
                }

                foreach (var s in data.SalaryDetails)
                {
                    currentRow++;
                    sheet.Cell(currentRow, 1).Value = s.MonthName;
                    sheet.Cell(currentRow, 2).Value = s.EarnedAmount;
                    sheet.Cell(currentRow, 3).Value = s.ActualDue;
                    sheet.Cell(currentRow, 4).Value = s.Note;
                }
                currentRow += 3;
                sheet.Cell(currentRow, 1).Value = "تفاصيل عمولات الاشتراكات";
                sheet.Cell(currentRow, 1).Style.Font.Bold = true;
                currentRow++;

                var subHeaders = new[] { "اسم اللاعب", "الرياضة", "قيمة الاشتراك", "المستحق الكلي", "المستحق لتاريخه" };
                for (int i = 0; i < subHeaders.Length; i++)
                {
                    var cell = sheet.Cell(currentRow, i + 1);
                    cell.Value = subHeaders[i];
                    cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                    cell.Style.Font.Bold = true;
                }

                foreach (var d in data.Details)
                {
                    currentRow++;
                    sheet.Cell(currentRow, 1).Value = d.PlayerName;
                    sheet.Cell(currentRow, 2).Value = d.SportName;
                    sheet.Cell(currentRow, 3).Value = d.PaymentValue;
                    sheet.Cell(currentRow, 4).Value = d.AmountForMonth;
                    sheet.Cell(currentRow, 5).Value = d.EarnedUntilNow;
                }

                currentRow += 3;
                sheet.Cell(currentRow, 1).Value = "سجل المسحوبات المالية";
                sheet.Cell(currentRow, 1).Style.Font.Bold = true;
                currentRow++;

                sheet.Cell(currentRow, 1).Value = "التاريخ";
                sheet.Cell(currentRow, 2).Value = "المبلغ";
                sheet.Cell(currentRow, 3).Value = "البيان";
                sheet.Range(currentRow, 1, currentRow, 3).Style.Fill.BackgroundColor = XLColor.LightSalmon;

                foreach (var c in data.CreditDetails)
                {
                    currentRow++;
                    sheet.Cell(currentRow, 1).Value = c.Date.ToString("yyyy/MM/dd");
                    sheet.Cell(currentRow, 2).Value = c.CreditValue;
                    sheet.Cell(currentRow, 3).Value = c.Description;
                }

                sheet.Columns().AdjustToContents();
                workbook.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تصدير الإكسل: " + ex.Message);
            }
        }
        private void WriteSummaryHeader(IXLWorksheet sheet, TrainerDueses dues)
        {
            sheet.Cell(1, 1).SetValue("تقرير مالي تفصيلي للمدرب:");
            sheet.Cell(1, 2).SetValue(dues.Trainer?.FullName ?? "غير محدد");
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 14;

            var summary = new[] {
        new { Key = "تاريخ التقرير", Value = (object)dues.IssueDate.ToShortDateString(), Color = XLColor.Black },
        new { Key = "الرصيد المدوّر", Value = (object)dues.BalanceForward, Color = XLColor.Black },
        new { Key = "إجمالي الرواتب (ذمة)", Value = (object)dues.Salaries, Color = XLColor.Blue },
        new { Key = "إجمالي العمولات", Value = (object)dues.TotalSubscriptions, Color = XLColor.Green },
        new { Key = "إجمالي المسحوبات", Value = (object)dues.Credits, Color = XLColor.Red },
        new { Key = "الرصيد النهائي (الذمة الكلية)", Value = (object)dues.FinalBalance, Color = XLColor.DarkBlue },
        new { Key = "المستحق الفعلي حالياً", Value = (object)dues.TotalSalaryDebt, Color = XLColor.DarkGreen }
    };

            for (int i = 0; i < summary.Length; i++)
            {
                int row = i + 3;
                sheet.Cell(row, 1).SetValue(summary[i].Key);

                sheet.Cell(row, 2).SetValue(summary[i].Value.ToString());

                sheet.Cell(row, 2).Style.Font.FontColor = summary[i].Color;
                sheet.Cell(row, 2).Style.Font.Bold = true;
            }
        }
        public List<TrainerDueses> ImportFromExcel(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
