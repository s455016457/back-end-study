using System.Collections.Generic;
using System.Drawing.Printing;

namespace Common.Print
{
    /// <summary>
    /// 打印机帮助类
    /// </summary>
    public class PrinterHelper
    {
        /// <summary>
        /// 获取默认打印机
        /// </summary>
        /// <returns></returns>
        public static string DefaultPrinter()
        {
            return new PrintDocument().PrinterSettings.PrinterName;
        }

        /// <summary>
        /// 获取已安装打印机列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLocalPrinters()
        {
            List<string> fPrinters = new List<string>();
            fPrinters.Add(DefaultPrinter()); //默认打印机始终出现在列表的第一项
            foreach (string fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!fPrinters.Contains(fPrinterName))
                {
                    fPrinters.Add(fPrinterName);
                }
            }
            return fPrinters;
        }
    }
}
