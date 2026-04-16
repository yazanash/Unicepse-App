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
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Uniceps.Views
{
    /// <summary>
    /// Interaction logic for PrintWindowDialog.xaml
    /// </summary>
    public partial class PrintWindowDialog : Window
    {
        string fileName = "file_name";
        public PrintWindowDialog()
        {
            InitializeComponent();


        }
        public PrintWindowDialog(string fileName)
        {
            InitializeComponent();
            this.fileName = fileName;

        }
        private void preview()
        {
            //if (File.Exists("print_preview.xps") == true) File.Delete("print_preview.xps");

            //XpsDocument xpsDocument = new XpsDocument("print_preview.xps", FileAccess.ReadWrite);
            //XpsDocumentWriter xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
            //SerializerWriterCollator serializerWriterCollator =xpsDocumentWriter.CreateVisualsCollator();
            //serializerWriterCollator.BeginBatchWrite();
            //serializerWriterCollator.WriteAsync(print);
            //serializerWriterCollator.EndBatchWrite();

            //FixedDocumentSequence fixedDocumentSequence = xpsDocument.GetFixedDocumentSequence();
            //xpsDocument.Close();
            //var window = new Window();
            //window.Content=new DocumentViewer { Document = fixedDocumentSequence };
            //window.ShowDialog();

            //xpsDocumentWriter = null;
            //serializerWriterCollator=null;
            //xpsDocument = null;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //PrintDialog printDialog = new PrintDialog();
            //if (printDialog.ShowDialog() == true)
            //{
            //    myscroll.ScrollToTop();
            //    myscroll.UpdateLayout();

            //    System.Printing.PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);

            //    Size pageSize = new Size(794, 280);

            //    print.Measure(pageSize);
            //    print.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));

            //    printDialog.PrintTicket.PageMediaSize = new System.Printing.PageMediaSize(794, 280);

            //    // 5. تنفيذ الطباعة
            //    printDialog.PrintVisual(print, fileName);

            //    this.Close();
            //}
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                myscroll.ScrollToTop();
                myscroll.UpdateLayout();

                // 1. ضبط التوجيه للعرض (Landscape)
                printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;

                // 2. تحديد حجم الورقة المخصص (العرض 794 والارتفاع 280)
                printDialog.PrintTicket.PageMediaSize = new System.Printing.PageMediaSize(794, 280);

                // 3. المعالجة والترتيب (المهمة جداً لضبط الهوامش)
                Size pageSize = new Size(794, 280);
                print.Measure(pageSize);
                print.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));

                // 4. تنفيذ الطباعة
                printDialog.PrintVisual(print, fileName);

                this.Close();
            }
        }
    }
}
