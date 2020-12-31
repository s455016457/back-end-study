using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;

namespace WebApi.Controllers.Download
{
    /// <summary>
    /// 文件下载
    /// </summary>
    [RoutePrefix("api/Download")]//定义特性路由前缀
    public class DownloadController : ApiController
    {
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件地址</param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [Route("DownloadFile")]
        public HttpResponseMessage DownloadFile(string filePath)
        {
            if (filePath.Length == 0) throw new Exception("文件地址不能为空！");
            var downloadEntity = new DownloadFileEntity(Path.Combine("Download", filePath));
            SetDwonloadEntity(Request, downloadEntity);

            return new HttpDownResponseMessage(downloadEntity);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件地址</param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [Route("DownloadVideo")]
        public HttpResponseMessage DownloadVideo(string filePath)
        {
            if (filePath.Length == 0) throw new Exception("视频地址不能为空！");
            var downloadEntity = new DownloadVideoEntity(Path.Combine("Download", filePath));
            SetDwonloadEntity(Request, downloadEntity);

            return new HttpDownResponseMessage(downloadEntity);
        }

        private void SetDwonloadEntity(HttpRequestMessage request, AbstractDownloadEntity dwonloadEntity)
        {
            var ranges = request.Headers.Range;
            if (ranges == null) return;
            var range = ranges.Ranges.FirstOrDefault();
            if (range == null) return;

            if (range.From.HasValue) dwonloadEntity.From = range.From.Value;
            if (range.To.HasValue) dwonloadEntity.To = range.To.Value;
        }
    }
    /// <summary>
    /// 下载抽象类
    /// </summary>
    public abstract class AbstractDownloadEntity
    {
        /// <summary>
        /// 
        /// </summary>
        protected const string MimeType = "application/octet-stream";
        /// <summary>
        /// 文件的长度
        /// </summary>
        public long FileLength { get; private set; }
        /// <summary>
        /// 开始
        /// </summary>
        public long From { get; set; } = 0;
        /// <summary>
        /// 结束
        /// </summary>
        public long To { get; set; }
        /// <summary>
        /// 数据流长度
        /// </summary>
        public long StreamLength { get { return To - From; } }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; private set; }
        /// <summary>
        /// 缓冲大小
        /// </summary>
        public int BufferSize { get; private set; } = 80 * 1024;
        /// <summary>
        /// 是否已完成下载
        /// </summary>
        public bool IsDownloadComplete { get { return true; } }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; private set; }
        private FileInfo fileInfo;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="FilePath"></param>
        public AbstractDownloadEntity(string FilePath)
        {
            this.FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FilePath);
            if (!File.Exists(this.FilePath))
            {
                throw new Exception($"文件{FilePath}不存在！");
            }
            fileInfo = new FileInfo(this.FilePath);
            To = FileLength = fileInfo.Length;
            FileName = fileInfo.Name;
        }
        /// <summary>
        /// 获取流
        /// </summary>
        /// <returns></returns>
        public virtual HttpContent GetHttpContent()
        {
            var content = CreateHttpContent(GetStream());
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            content.Headers.ContentDisposition.FileName = FileName;
            content.Headers.ContentDisposition.Size = FileLength;
            content.Headers.ContentDisposition.CreationDate = fileInfo.CreationTime;
            content.Headers.ContentDisposition.DispositionType = fileInfo.Extension;
            content.Headers.ContentDisposition.ModificationDate = fileInfo.LastWriteTime;
            content.Headers.ContentDisposition.ReadDate = fileInfo.LastAccessTime;
            content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);
            content.Headers.ContentLength = FileLength;
            if (!IsDownloadComplete)
            {
                content.Headers.ContentRange = new ContentRangeHeaderValue(From, To, FileLength);
            }
            return content;
        }
        private Stream GetStream()
        {
            FileStream fileStream = File.OpenRead(FilePath);
            fileStream.Seek(From, SeekOrigin.Begin);
            return fileStream;
        }
        /// <summary>
        /// 创建httpcontent
        /// </summary>
        /// <returns></returns>
        protected abstract HttpContent CreateHttpContent(Stream stream);
    }
    /// <summary>
    /// 下载文件
    /// </summary>
    public class DownloadFileEntity: AbstractDownloadEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        public DownloadFileEntity(string FilePath) : base(FilePath) { }
        /// <summary>
        /// 创建httpcontent
        /// </summary>
        /// <returns></returns>
        protected override HttpContent CreateHttpContent(Stream stream)
        {
            var content = new StreamContent(stream, BufferSize);
            return content;
        }
    }
    /// <summary>
    /// 下载视频
    /// </summary>
    public class DownloadVideoEntity : AbstractDownloadEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        public DownloadVideoEntity(string FilePath) : base(FilePath) { }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override HttpContent CreateHttpContent(Stream stream)
        {
            var content = new PushStreamContent((Stream, HttpContentm,transportContext)=> { }, MimeType);
            return content;
        }

        private void callback(Stream stream, HttpContent httpContent, TransportContext transportContext)
        {
        }
    }

    /// <summary>
    /// 下载响应
    /// </summary>
    public class HttpDownResponseMessage : HttpResponseMessage
    {
        /// <summary>
        /// 初始化一个下载响应
        /// </summary>
        public HttpDownResponseMessage(AbstractDownloadEntity downloadEnity) : base()
        {
            InitResponse(downloadEnity);
        }

        /// <summary>
        /// 初始化一个下载响应
        /// </summary>
        public HttpDownResponseMessage(AbstractDownloadEntity downloadEnity,HttpStatusCode httpStatusCode) : base(httpStatusCode)
        {
            InitResponse(downloadEnity);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="downloadEnity"></param>
        private void InitResponse(AbstractDownloadEntity downloadEnity)
        {
            Headers.AcceptRanges.Add("bytes");
            StatusCode = downloadEnity.IsDownloadComplete ? HttpStatusCode.OK : HttpStatusCode.PartialContent;
            Content = downloadEnity.GetHttpContent();
        }
    }
}
