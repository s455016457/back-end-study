using System;
using System.Web;

namespace WebPrintClient
{
   public  class Program
    {
        public static void Main(string[] args)
        {
            FluentConsole.Green.Line("Web Print Client");
            if (args != null && args.Length > 0)
            {
                var str = args[0];
                str = HttpUtility.UrlDecode(str);
#if DEBUG
                FluentConsole.Gray.Line(str);
#endif

                str = str.Substring(str.IndexOf(":") + 1);

                var command = Command.FromString(str);

                command.Execute();
            }
            //var defaultPrinter = PrinterHelper.DefaultPrinter();
            //var printers = PrinterHelper.GetLocalPrinters();

            //Console.WriteLine($"默认打印机【{defaultPrinter}】");

            //Console.WriteLine("系统已安装打印机：");
            //foreach(var printName in printers)
            //{
            //    Console.WriteLine(printName);
            //}

            //Console.WriteLine("PDF 测试");

            //var pages = PDFHelper.ToImages("TestDocument/ASP.NET WEB MVC生命周期.pdf");

            //CPrintDocument cPrintDocument = new CPrintDocument(defaultPrinter,new PaperSize ("A4",297,210));
            //cPrintDocument.Print(pages);
            FluentConsole.Green.Line("按任意键退出...");
            Console.Read();
        }
    }
}
