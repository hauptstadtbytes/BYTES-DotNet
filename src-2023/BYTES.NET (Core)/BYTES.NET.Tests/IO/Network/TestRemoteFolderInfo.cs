using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BYTES.NET.IO;
using System.IO;
using BYTES.NET.IO.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.SqlServer.Server;
using System.Diagnostics;

namespace BYTES.NET.Tests.IO.Network
{
    [TestClass]
    public class TestRemoteFolderInfo
    {
        RemoteConnection conn;
        UserInfo user;
        RemoteFolderInfo folderInfo;
        string fileSharePath;
        string tempPath;
        string tempFolderPath;

        [TestInitialize]
        public void Setup()
        {
             IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<TestRemoteFolderInfo>()
                .Build();

            fileSharePath = config["FileShare:Domain"];

            user = new UserInfo(config["FileShare:Username"], config["FileShare:Password"]);
            conn = new RemoteConnection(fileSharePath, user);

            folderInfo = new RemoteFolderInfo(fileSharePath, user);

            tempPath = Path.Combine(fileSharePath, "tempFolder");
            Directory.CreateDirectory(tempPath);
            Directory.CreateDirectory(tempPath + "\\MovedFiles\\");

            tempFolderPath = Path.Combine(fileSharePath, "TestFolder");
            Directory.CreateDirectory(tempFolderPath);
            File.WriteAllText(Path.Combine(tempFolderPath, "test1.txt"), "File 1");
            File.WriteAllText(Path.Combine(tempFolderPath, "test2.txt"), "File 2");
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(tempPath, true);
            Directory.Delete(tempFolderPath, true);
        }

        [TestMethod]
        public void TestGetFolders()
        {
            Assert.IsNotNull(folderInfo.GetFolders());
        }

        [TestMethod]
        public void TestGetFiles()
        {
            string path = Path.Combine(fileSharePath, "TestFolder");
            Assert.IsNotNull(folderInfo.GetFiles(path));
        }

        [TestMethod]
        public void TestCopyFileToAndFileExists()
        {
            string newFile = "\\" + Guid.NewGuid();
            File.Create(tempPath + newFile).Dispose();

            folderInfo.CopyFileTo(tempPath + newFile, tempPath + "\\MovedFiles\\" + newFile);
            Assert.IsTrue(folderInfo.FileExists(tempPath + "\\MovedFiles\\" + newFile));
        }

        [TestMethod]
        public void TestGetFolderInfo()
        {
            Assert.IsNotNull(folderInfo.GetFolderInfo(fileSharePath));
        }

        [TestMethod]
        public void TestGetFileInfo()
        {
            string path = Path.Combine(fileSharePath, "TestFolder", "test1.txt");
            Assert.IsNotNull(folderInfo.GetFileInfo(path));
        }

        [TestMethod]
        public void TestFolderExists()
        {
            string path = Path.Combine(fileSharePath, "TestFolder");
            Assert.IsTrue(folderInfo.FolderExists(path));
        }

        [TestMethod]
        public void TestIsReadable()
        {
            Assert.IsTrue(folderInfo.IsReadable());
        }

        [TestMethod]
        public void TestReadBytes()
        {
            string path = Path.Combine(fileSharePath, "TestFolder", "test1.txt");
            byte[] t1 = File.ReadAllBytes(path);
            byte[] t2 = folderInfo.ReadBytes(path);

            Assert.AreEqual(BitConverter.ToString(t1), BitConverter.ToString(t2));
        }
    }
}
