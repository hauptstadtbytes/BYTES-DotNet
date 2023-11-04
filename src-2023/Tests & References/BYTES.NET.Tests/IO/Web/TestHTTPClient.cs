//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Web;
using BYTES.NET.IO;
using System.Diagnostics;

namespace BYTES.NET.Tests.IO.Web
{
    [TestClass]
    public class TestHTTPClient
    {
        private string rootURL = "https://mockup.bytesapp.de/RESTAPI";

        [TestMethod]
        public void TestGET()
        {
            HTTPClient client = new HTTPClient();

            string response = client.GET(rootURL + "/echo/parameters?hello=world!");

            Debug.WriteLine(response);

            Assert.AreEqual(true, response.Contains("\"hello\": \"world!\""));
        }

        [TestMethod]
        public void TestJSONPOST()
        {
            HTTPClient client = new HTTPClient();

            string response = client.POST(rootURL + "/echo/jsondata", "{\"hello\":\"world\",\"another\":\"value\"}");

            Debug.WriteLine(response);

            Assert.AreEqual(true, response.Contains("\"another\": \"value\""));
        }

        [TestMethod]
        public void TestFormDataPOST()
        {
            HTTPClient client = new HTTPClient();

            Dictionary<string, string> data = new Dictionary<string, string>() { { "myName", "Hello" }, { "myValue", "World!" } };

            string response = client.POST<string>(rootURL + "/echo/formdata", data);

            Debug.WriteLine(response);

            Assert.AreEqual(true, response.Contains("\"myValue\": \"World!\""));
        }

        [TestMethod]
        public void TestDownload()
        {
            HTTPClient client = new HTTPClient();

            string targetPath = "%BYTES.NET.DIR%\\..\\..\\..\\..\\..\\Sample Data\\DownloadedFile.pdf";
            targetPath = targetPath.ExpandPath();

            if (File.Exists(targetPath))
            {
                File.Delete(targetPath );
            }

            client.Download("http://cdn.hauptstadtbytes.de/ADBs.pdf",targetPath);

            Assert.AreEqual(true, File.Exists(targetPath));
        }
    }
}
