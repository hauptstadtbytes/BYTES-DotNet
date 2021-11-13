//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

//import namespace(s) required from 'BYTES.NET' library
using BYTES.NET.MEF;

//import internal namespace(s) required
using BYTES.NET.TEST.MEF.API;

namespace BYTES.NET.TEST.MEF
{

    [TestClass]
    public class TestExtensionsManager
    {

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void CheckSourceParsing()
        {

            //create a new manager instance
            ExtensionsManager<ITestInterface, ITestMetadata> manager = new ExtensionsManager<ITestInterface, ITestMetadata>();

            //get the extension(s) using a Win file path
            Uri source = null;
            if (Uri.TryCreate(System.Reflection.Assembly.GetExecutingAssembly().CodeBase, UriKind.RelativeOrAbsolute, out source))
            {

                TestContext.WriteLine(source.ToString() + " parsed successfully");

                string[] paths1 = { source.LocalPath };
                manager.Update(paths1);

                TestContext.WriteLine(manager.Extensions.Length.ToString() + " extension(s) found using local path");

                Assert.AreEqual(1, manager.Extensions.Length);

                string[] paths2 = {source.ToString()};
                manager.Update(paths2);

                TestContext.WriteLine(manager.Extensions.Length.ToString() + " extension(s) found using URI path");

                Assert.AreEqual(1, manager.Extensions.Length);

            }

        }

    }

}
