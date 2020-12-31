using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace WebPrintClient
{
    public class PDFHelper
    {
        public static IEnumerator<Image> ToImages(string filePath)
        {
            using (PdfDocument doc = new PdfDocument())
            {
                doc.LoadFromFile(filePath);

                for (int i = 0; i < doc.Pages.Count; i++)
                {
                    var page = doc.Pages[i];
                    Console.WriteLine($"真实尺寸【{page.ActualSize.Width}*{page.ActualSize.Height}】 文档尺寸【{page.Size.Width}*{page.Size.Height}】");
                    yield return Image.FromStream(doc.SaveAsImage(i, PdfImageType.Bitmap));
                }
            }
        }

        public static IEnumerator<Image> ToImages(Stream stream)
        {
            using (PdfDocument doc = new PdfDocument(stream))
            {
                for (int i = 0; i < doc.Pages.Count; i++)
                {
                    var page = doc.Pages[i];
                    Console.WriteLine($"真实尺寸【{page.ActualSize.Width}*{page.ActualSize.Height}】 文档尺寸【{page.Size.Width}*{page.Size.Height}】");
                    yield return Image.FromStream(doc.SaveAsImage(i, PdfImageType.Bitmap));
                }
            }
        }
    }
}
