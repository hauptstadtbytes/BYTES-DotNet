//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' library
using BYTES.NET.IO;


namespace BYTES.NET.TEST.IO
{

    [TestClass]
    public class TestIOHelper
    {

        [TestMethod]
        public void ValidateDefaultPaths()
        {

            Assert.AreEqual(AppDomain.CurrentDomain.BaseDirectory, Helper.GetAppDirPath()); //validate the installation root dir

            Assert.AreEqual(Helper.GetLibraryAssemblyPath(), Helper.ExpandPath("%BYTES.NET%")); //validate the path parsing

        }

    }
}
