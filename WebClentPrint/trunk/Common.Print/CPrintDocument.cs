using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;

namespace Common.Print
{
    /// <summary>
    /// 打印机文档
    /// </summary>
    public class CPrintDocument
    {
        public string DocumentName { get; set; } = "Default Document";
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; }
        /// <summary>
        /// 打印纸张大小
        /// </summary>
        public PaperSize PaperSize { get; set; }

        public CPrintDocument() { }

        public CPrintDocument(string printerName, PaperSize paperSize)
        {
            PrinterName = printerName;
            PaperSize = paperSize;
        }

        public void Print(Image image)
        {
            using (var pd = new PrintDocument())
            {
                // 设置打印机
                pd.PrinterSettings.PrinterName = PrinterName ?? PrinterHelper.DefaultPrinter();
                // 设置纸张格式
                pd.DefaultPageSettings.PaperSize = PaperSize.ToPrintPaperSize();
                pd.DocumentName = DocumentName;

                pd.PrintPage += (sender, e) =>
                {
                    e.Graphics.DrawImage(image, e.MarginBounds);
                };

                pd.Print();
            }
        }

        public void Print(IEnumerator<Image> images)
        {
            using (var pd = new PrintDocument())
            {
                // 设置打印机
                pd.PrinterSettings.PrinterName = PrinterName ?? PrinterHelper.DefaultPrinter();
                // 设置纸张格式
                pd.DefaultPageSettings.PaperSize = PaperSize.ToPrintPaperSize();
                pd.DocumentName = DocumentName;

                var index = 0;
                images.MoveNext();
                pd.PrintPage += (sender, e) =>
                {
                    var image = images.Current;

                    Rectangle m = e.MarginBounds;
                    m.Y += index++ * (m.Y + m.Height);

                    e.Graphics.DrawImage(image, e.MarginBounds);

                    if (images.MoveNext() && !e.HasMorePages)
                    {
                        e.HasMorePages = true;
                    }
                    else
                    {
                        e.HasMorePages = false;
                    }
                };

                pd.Print();
            }
        }

        public static PaperSize GetPrinterDefaultPaperSize(string printerName)
        {
            using (var pd = new PrintDocument())
            {
                return PaperSize.FromPrintPaperSize(pd.DefaultPageSettings.PaperSize);
            }
        }
    }
}
