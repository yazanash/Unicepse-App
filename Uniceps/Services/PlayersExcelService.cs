using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Player;

namespace Uniceps.Services
{
    internal class PlayersExcelService : IExcelService<Player>
    {
        private readonly Dictionary<string, int> columnOrder = new()
        {
            // personal information
            ["Id"] = 1,
            ["FullName"] = 2,
            ["Phone"] = 3,
            ["BirthDate"] = 4,
            ["Gender"] = 5,
            ["Weight"] = 6,
            ["Hieght"] = 7,
            ["SubscribeDate"] = 8,
            ["SubscribeEndDate"] = 9,
            ["Balance"] = 10,
        };

        public void ExportToExcel(string filePath, IEnumerable<Player> data)
        {
            try
            {

                using var workbook = new XLWorkbook();

                var sheet = workbook.Worksheets.Add("Sheet1");

                WriteHeaders(sheet);

                int rowIndex = 2;
                foreach (var p in data)
                {
                    var row = sheet.Row(rowIndex);
                    WritePlayer(row, sheet, p); // Write personal information 
                    rowIndex++;
                }
                workbook.SaveAs(filePath);

            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ في تصدير ملف الاكسل .... تفاصيل الخطأ " + ex.Message);
            }
        }
        private void WriteHeaders(IXLWorksheet sheet)
        {
            foreach (var kvp in columnOrder)
            {
                sheet.Cell(1, kvp.Value).Value = kvp.Key;
            }
        }

        private void WritePlayer(IXLRow row, IXLWorksheet sheet, Player b)
        {
            Set(sheet, row, "Id", b.Id);
            Set(sheet, row, "FullName", b.FullName);
            Set(sheet, row, "Phone", b.Phone);
            Set(sheet, row, "BirthDate", b.BirthDate);
            Set(sheet, row, "Weight", b.Weight);
            Set(sheet, row, "Hieght", b.Hieght);
            Set(sheet, row, "SubscribeDate", b.SubscribeDate.ToShortDateString());
            Set(sheet, row, "SubscribeEndDate", b.SubscribeEndDate.ToShortDateString());
            Set(sheet, row, "Balance", b.Balance);
        }
        private void Set(IXLWorksheet sheet, IXLRow row, string columnKey, object? value)
        {
            if (columnOrder.TryGetValue(columnKey, out int col))
            {
                sheet.Cell(row.RowNumber(), col).Value = value?.ToString();
            }
        }
        public List<Player> ImportFromExcel(string filePath)
        {
            try
            {
                var players = new List<Player>();
                var workbook = new XLWorkbook(filePath);
                var worksheet = workbook.Worksheet(1);
                //ٌRead Columns
                var headerRow = worksheet.Row(1);
                var columnMap = new Dictionary<string, int>();

                foreach (var cell in headerRow.CellsUsed())
                {
                    var columnName = cell.GetString().Trim();
                    var columnIndex = cell.Address.ColumnNumber;
                    columnMap[columnName] = columnIndex;
                }


                foreach (var row in worksheet.RowsUsed().Skip(1)) // تخطي رأس الجدول
                {
                    Player player = GetPlayer(row, columnMap);
                    players.Add(player);
                }
                return players;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ في استيراد ملف الاكسل .... تفاصيل الخطأ " + ex.Message);
                return new List<Player>();
            }
        }
        public Player GetPlayer(IXLRow row, Dictionary<string, int> columnMap)
        {
            Player player = new Player
            {
                FullName = row.Cell(columnMap["FullName"]).GetString(),
                Phone = row.Cell(columnMap["Phone"]).GetString(),
                BirthDate = TryGetInt(row, columnMap, "BirthDate"),
                GenderMale = TryGetBool(row, columnMap, "Gender"),
                Weight = TryGetInt(row, columnMap, "Weight"),
                Hieght = TryGetInt(row, columnMap, "Hieght"),
                SubscribeDate = TryGetDate(row, columnMap, "SubscribeDate") ?? DateTime.Now,
                SubscribeEndDate = TryGetDate(row, columnMap, "SubscribeEndDate") ?? DateTime.Now
            };
            return player;
        }
        int TryGetInt(IXLRow row, Dictionary<string, int> map, string column)
        {
            if (map.TryGetValue(column, out int index))
            {
                var value = row.Cell(index).GetString().Trim();
                if (int.TryParse(value, out int result))
                    return result;
            }
            return 0;
        }
        DateTime? TryGetDate(IXLRow row, Dictionary<string, int> map, string column)
        {
            if (map.TryGetValue(column, out int index))
            {
                var value = row.Cell(index).GetString().Trim();
                if (DateTime.TryParse(value, out DateTime result))
                    return result;
            }
            return null;
        }
        string? TryGetText(IXLRow row, Dictionary<string, int> map, string column)
        {
            if (map.TryGetValue(column, out int index))
            {
                var value = row.Cell(index).GetString().Trim();
                return string.IsNullOrWhiteSpace(value) ? null : value;
            }
            return null;
        }
        bool TryGetBool(IXLRow row, Dictionary<string, int> map, string column)
        {
            if (map.TryGetValue(column, out int index))
            {
                var value = row.Cell(index).GetString().Trim().ToLower();
                if (value == "yes" || value == "true" || value == "1") return true;
                if (value == "no" || value == "false" || value == "0") return false;
            }
            return false;
        }
    }
}
