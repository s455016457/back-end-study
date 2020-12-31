using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ZXing;
using ZXing.QrCode;

namespace TestPDFsharp
{
    public class Program
    {
      private   static XGraphicsState state;

        public static void Main(string[] args)
        {
            RenderQRCoder();
            CreatPDF();
            Console.WriteLine("Hello world");
        }


        private static void CreatPDF()
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Create a font
            XFont font = new XFont("Times", 25, XFontStyle.Regular);

            //PageSize[] pageSizes = (PageSize[])Enum.GetValues(typeof(PageSize));
            //foreach (PageSize pageSize in pageSizes)
            //{
            //    if (pageSize == PageSize.Undefined)
            //        continue;

            //    // One page in Portrait...
            //    PdfPage page = document.AddPage();
            //    page.Size = pageSize;
            //    XGraphics gfx = XGraphics.FromPdfPage(page);
            //    gfx.DrawString(pageSize.ToString(), font, XBrushes.DarkRed,
            //      new XRect(0, 0, page.Width, page.Height),
            //      XStringFormats.Center);

            //    // ... and one in Landscape orientation.
            //    page = document.AddPage();
            //    page.Size = pageSize;
            //    page.Orientation = PageOrientation.Landscape;
            //    gfx = XGraphics.FromPdfPage(page);
            //    gfx.DrawString(pageSize + " (landscape)", font,
            //      XBrushes.DarkRed, new XRect(0, 0, page.Width, page.Height),
            //      XStringFormats.Center);
            //}
            
            PdfPage myPage = document.AddPage();
            myPage.Width = new XUnit(7,XGraphicsUnit.Centimeter);
            myPage.Height = new XUnit(5,XGraphicsUnit.Centimeter);
           var  myGfx = XGraphics.FromPdfPage(myPage);
            myGfx.DrawString("7cm*5cm", font,
              XBrushes.DarkRed, new XRect(2.5, 0.2, myPage.Width, myPage.Height),
              XStringFormats.TopLeft);

            DrawImage(myGfx, 1);

            //myPage = document.AddPage();
            //myPage.Width = new XUnit(70, XGraphicsUnit.Millimeter);
            //myPage.Height = new XUnit(50, XGraphicsUnit.Millimeter);
            //myGfx = XGraphics.FromPdfPage(myPage);
            //myGfx.DrawString("70*50", font,
            //  XBrushes.DarkRed, new XRect(0, 0, myPage.Width, myPage.Height),
            //  XStringFormats.Center);

            //DrawImage(myGfx, 1);


            // Save the document...
            const string filename = "PageSizes_tempfile.pdf";
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }

       static  void DrawImage(XGraphics gfx, int number)
        {
            //BeginBox(gfx, number, "DrawImage (original)");

            XImage image = XImage.FromFile("11.jpg");

            // Left position in point
            double x = (250 - image.PixelWidth * 72 / image.HorizontalResolution) / 2;
            gfx.DrawImage(image, 2, 2, 25*2.54,25*2.54);

            //EndBox(gfx);
        }

        public static void BeginBox(XGraphics gfx, int number, string title)
        {
            const int dEllipse = 15;
            XRect rect = new XRect(0, 20, 300, 200);
            if (number % 2 == 0)
                rect.X = 300 - 5;
            rect.Y = 40 + ((number - 1) / 2) * (200 - 5);
            rect.Inflate(-10, -10);
            XRect rect2 = rect;
            rect2.Offset(1,1);
            gfx.DrawRoundedRectangle(new XSolidBrush(XColor.FromName("red")), rect2, new XSize(dEllipse + 8, dEllipse + 8));
            XLinearGradientBrush brush = new XLinearGradientBrush(rect, XColor.FromName("back"), XColor.FromName("back"), XLinearGradientMode.Vertical);
            gfx.DrawRoundedRectangle(new XPen(XColor.FromName("blu"),1), brush, rect, new XSize(dEllipse, dEllipse));
            //rect2.Offset(this.borderWidth, this.borderWidth);
            //gfx.DrawRoundedRectangle(new XSolidBrush(this.shadowColor), rect2, new XSize(dEllipse + 8, dEllipse + 8));
            //XLinearGradientBrush brush = new XLinearGradientBrush(rect, this.backColor, this.backColor2, XLinearGradientMode.Vertical);
            //gfx.DrawRoundedRectangle(this.borderPen, brush, rect, new XSize(dEllipse, dEllipse));
            rect.Inflate(-5, -5);

            XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
            gfx.DrawString(title, font, XBrushes.Navy, rect, XStringFormats.TopCenter);

            rect.Inflate(-10, -5);
            rect.Y += 20;
            rect.Height -= 20;

            Program.state = gfx.Save();
            gfx.TranslateTransform(rect.X, rect.Y);
        }

        public static void EndBox(XGraphics gfx)
        {
            gfx.Restore(Program.state);
        }

        private static void RenderQRCoder()
        {
            string context = @"ASDFEDDSADEFDDWDFASDFESDFDESDFESDFESDFESDSSDFEWDSEF,IUKUSFEKJKLIJKLIJKOF,IUKUSFEKJKLIJKLIJKOF,1000000000000000000";
            if (context.Length == 0) return;
            //QRCodeWriter qRCodeWriter = new ZXing.QrCode.QRCodeWriter();
            //var bitMatrix = qRCodeWriter.encode(context, BarcodeFormat.QR_CODE, pictureBox1.Width - 10, pictureBox1.Height - 10);
            new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    CharacterSet = "UTF-8",
                    Margin = 1,
                    DisableECI = true,
                    Width = (int)(250*2.54),
                    Height = (int)(250 * 2.54),
                    QrVersion =10,
                    ErrorCorrection=ZXing.QrCode.Internal.ErrorCorrectionLevel.L
                }
            }.Write(context)
            .Save("11.jpg");
        }
        
        public class CEELevel
        {
            public const string L = "L";
            public const string Q = "Q";
            public const string M = "M";
            public const string H = "H";
        }
    }
}
