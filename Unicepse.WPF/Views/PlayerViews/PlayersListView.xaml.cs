using Microsoft.Win32;
using Unicepse.WPF.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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


namespace Unicepse.WPF.Views.PlayerViews
{
    /// <summary>
    /// Interaction logic for PlayersListView.xaml
    /// </summary>
    public partial class PlayersListView : UserControl
    {
        public PlayersListView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel._Application application = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = application.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet? worksheet = null;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "player details";
                worksheet.Cells.ColumnWidth = 25;
                for (int i = 1; i < PlayerDataGrid.Columns.Count ; i++)
                {
                    worksheet.Cells[1, i] = PlayerDataGrid.Columns[i - 1].Header.ToString();

                }
                for (int i = 0; i < PlayerDataGrid.Items.Count; i++)
                {
                    var item = PlayerDataGrid.Items[i] as PlayerListItemViewModel;
                    worksheet.Cells[i + 2, 1] = item!.Id;
                    worksheet.Cells[i + 2, 2] = item!.FullName;
                    worksheet.Cells[i + 2, 3] = item!.BirthDate;
                    worksheet.Cells[i + 2, 4] = item!.Gendertext;
                    worksheet.Cells[i + 2, 5] = item!.SubscribeDate;

                    worksheet.Cells[i + 2, 6] = item!.SubscribeEndDate;
                    worksheet.Cells[i + 2, 7] = item!.DayLeft;
                    worksheet.Cells[i + 2, 8] = item!.Balance;
                }
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "platinum";
                saveFileDialog.DefaultExt = ".xlsx";
                saveFileDialog.Filter = "Excel spreadsheet (*.xlsx)|*.xlsx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing);
                }
                application.Quit();
            }
            catch
            {
                MessageBox.Show("Excel not installed we will print it with CSV Format");
                  WriteCSVFile();
            }

          
        }

        public void WriteCSVFile()
        {
            StringBuilder csvBuilder = new StringBuilder();
            for (int i = 1; i < PlayerDataGrid.Columns.Count + 1; i++)
            {
                csvBuilder.Append(PlayerDataGrid.Columns[i - 1].Header.ToString() + ",");

            }
            csvBuilder.AppendLine();

            foreach (var item in PlayerDataGrid.Items)
            {
                var player = item as PlayerListItemViewModel;
                csvBuilder.Append(player!.Id + ",");
                csvBuilder.Append(player!.FullName + ",");
                csvBuilder.Append(player!.BirthDate + ",");
                csvBuilder.Append(player!.Gendertext + ",");
                csvBuilder.Append(player!.SubscribeDate!.Replace(',','-') + ",");

                csvBuilder.Append(player!.SubscribeEndDate!.Replace(',', '-') + ",");
                csvBuilder.Append(player!.DayLeft + ",");
                csvBuilder.Append(player!.Balance + ",");
              
                csvBuilder.AppendLine();
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "platinumcsv";
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.Filter = "Comma-separated values file (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, csvBuilder.ToString(), Encoding.UTF8);
            }
        }
    }
}
