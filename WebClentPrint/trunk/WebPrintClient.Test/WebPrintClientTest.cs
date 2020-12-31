using NUnit.Framework;
using WebPrintClient;
namespace WebPrintClient.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Program.Main(new string[] { "WebPrintClient:GetPrinterList http://localhost/SWERP/login.aspx" });
        }
    }
}