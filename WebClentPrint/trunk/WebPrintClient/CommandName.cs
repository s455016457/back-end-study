using System;
using System.Collections.Generic;
using System.Text;

namespace WebPrintClient
{
    /// <summary>
    /// 命令名称
    /// </summary>
    public enum CommandName
    {
        /// <summary>
        /// 获取打印机列表
        /// </summary>
        GetPrinterList,
        /// <summary>
        /// 获取默认打印机
        /// </summary>
        GetDefaultPrinter,
        /// <summary>
        /// 打印
        /// </summary>
        DoPrint
    }
}
