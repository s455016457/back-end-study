namespace Common.Print
{
    /// <summary>
    /// 纸张大小
    /// </summary>
    public struct PaperSize
    {
        /// <summary>
        /// 纸张名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 纸张宽度
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 纸张高度
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">纸张名称</param>
        /// <param name="width">纸张宽度 单位毫米</param>
        /// <param name="height">纸张高度 单位毫米</param>
        public PaperSize(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
        }

        public override string ToString() => $"[{Name}]({Width}, {Height})";

        /// <summary>
        /// 转成打印纸张
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Printing.PaperSize ToPrintPaperSize()
        {
            return new System.Drawing.Printing.PaperSize(Name, (int)(Width * 100 / 25.4), (int)(Height * 100 / 25.4));
        }

        public static PaperSize FromPrintPaperSize(System.Drawing.Printing.PaperSize paperSize)
        {
            return new PaperSize(paperSize.PaperName,(int)(paperSize.Width * 0.254), (int)(paperSize.Height * 0.254));
        }
    }
}
