using Common.Print;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WebPrintClient
{
    /// <summary>
    /// 命令
    /// </summary>
    public class Command
    {
        const string printerName_ParamterKey = "PrinterName";
        const string defaultPrinter_ParamterKey = "DefaultPrinter";
        const string localPrinters_ParamterKey = "LocalPrinters";
        const string paperSize_ResponseHeadersKey = "PaperSize";

        /// <summary>
        /// 命令名称
        /// </summary>
        public CommandName CommandName { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public IDictionary<string,object> RequestParamters { get; set; }
        /// <summary>
        /// 页面大小
        /// </summary>
        private PaperSize? PaperSize { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        public void Execute()
        {
            FluentConsole.Gray.Line($"执行命令【{Enum.GetName(typeof(CommandName), CommandName)}】");
            switch (CommandName)
            {
                case CommandName.DoPrint:
                    Print();
                    break;
                case CommandName.GetDefaultPrinter:
                    UploadStringAsync(new KeyValuePair<string, object>(defaultPrinter_ParamterKey, PrinterHelper.DefaultPrinter()));
                    break;
                case CommandName.GetPrinterList:
                    UploadStringAsync(new KeyValuePair<string, object>(localPrinters_ParamterKey, PrinterHelper.GetLocalPrinters()));
                    break;
            }
            FluentConsole.Gray.Line("命令执行完成");
        }

        private void UploadStringAsync(KeyValuePair<string, object> data)
        {
            var paramter = new Dictionary<string, object>(RequestParamters);
            paramter.Add(data.Key, data.Value);
            using (WebClient client = new WebClient())
            {
                client.UploadStringAsync(new Uri(RequestUrl), "POST", Newtonsoft.Json.JsonConvert.SerializeObject(paramter));
            }
        }

        private void Print()
        {
            var defaultPrinter = PrinterHelper.DefaultPrinter();
            var paramters = new Dictionary<string, object>(RequestParamters);
            if (paramters.ContainsKey(printerName_ParamterKey))
            {
                defaultPrinter = paramters[printerName_ParamterKey].ToString();
                paramters.Remove(printerName_ParamterKey);
            }

            var stream = new MemoryStream();
            try
            {
                GetPrintDocumentStream(paramters, ref stream);
                var pages = PDFHelper.ToImages(stream);

                CPrintDocument cPrintDocument = new CPrintDocument(defaultPrinter
                    , PaperSize.HasValue ? PaperSize.Value : CPrintDocument.GetPrinterDefaultPaperSize(defaultPrinter));
                cPrintDocument.Print(pages);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取打印文档流
        /// </summary>
        /// <returns></returns>
        private void GetPrintDocumentStream(IDictionary<string ,object> paramters,ref MemoryStream stream)
        {
            var strParamter = Newtonsoft.Json.JsonConvert.SerializeObject(paramters);
            var request = WebRequest.Create(RequestUrl);
            request.Method = "POST";
            request.GetRequestStream().Write(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(strParamter)));

            using (var response = request.GetResponse())
            {
                var strPaperSize = response.Headers.Get(paperSize_ResponseHeadersKey);
                try
                {
                    PaperSize = Newtonsoft.Json.JsonConvert.DeserializeObject<PaperSize>(strPaperSize);
                }
                catch { }
                var bufferBytes = new byte[1024];
                var responseStream = response.GetResponseStream();
                var index = responseStream.Read(bufferBytes, 0, bufferBytes.Length);

                while (index > 0)
                {
                    stream.Write(bufferBytes, 0, index);
                    index = responseStream.Read(bufferBytes, 0, bufferBytes.Length);
                }
            }
        }

        public static Command FromString(string str)
        {
            var values = str.Split(" ");

            var command = new Command();
            for(var i = 0; i < values.Length; i++)
            {
#if DEBUG
                FluentConsole.Gray.Line(values[i]);
#endif
                switch (i)
                {
                    case 0:
                        try
                        {
                            command.CommandName = Enum.Parse<CommandName>(values[i]);
                        }catch
                        {
                            FluentConsole.Red.Line($"未知命令【{values[i]}】！");
                        }
                        break;
                    case 1:
                        var uri = new Uri (values[i]);
                        var strQuery = uri.Query;
                       command.RequestUrl = uri.OriginalString.Substring(0, uri.OriginalString.Length - strQuery.Length);
                        command.RequestParamters = ToParamters(strQuery);
                        break;
                }
            }

            return command;
        }

        private static Dictionary<string, object> ToParamters(string str)
        {
            var dic = new Dictionary<string, object>();
            if (string.IsNullOrWhiteSpace(str)) return dic;
            if (str[0].Equals('?'))
            {
                str = str.Substring(1);
            }
            var values  = str.Split("&");

            foreach(var item in values)
            {
                var v = item.Split("=");

                dic.Add(v[0], v[1]);
            }
            return dic;
        }
    }
}
