//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

//import .net namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Extensibility;

//import internal namespace(s) required
using BYTES.NET.Test.IO.Extensibility.API;

namespace BYTES.NET.Test.IO.Extensibility
{
    [TestClass]
    public class TestExtensibility
    {
        [TestMethod]
        public void TestSimpleEnumeration()
        {
            //enumerate, using the file path 'String' type
            ExtensionsManager<ITestInterface> manager = new ExtensionsManager<ITestInterface>();
            manager.Update(new string[] { "%BYTES.NET.DIR%\\BYTES.NET.Test.dll" });

            DumpExtensions<ExtensionsManager<ITestInterface>>(manager);
            Assert.AreEqual(3, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type
            manager = new ExtensionsManager<ITestInterface>();
            
            List<ExtensionsSource> sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET%"));
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<ITestInterface>>(manager);
            Assert.AreEqual(3, manager.Extensions.Length);
        }

        [TestMethod]
        public void TestAdvancedEnumeration()
        {
            //enumerate, using the file path 'String' type
            ExtensionsManager<ITestInterface, TestMetadata> manager = new ExtensionsManager<ITestInterface, TestMetadata>();
            manager.Update(new string[] { "%BYTES.NET.DIR%\\BYTES.NET.Test.dll" });

            DumpExtensions<ExtensionsManager<ITestInterface, TestMetadata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type (1)
            manager = new ExtensionsManager<ITestInterface, TestMetadata>();

            List<ExtensionsSource> sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET%"));
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<ITestInterface, TestMetadata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type and metadata filtering (1)
            manager = new ExtensionsManager<ITestInterface, TestMetadata>();

            sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll", "Name=TestImplementationTwo"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<ITestInterface, TestMetadata>>(manager);
            Assert.AreEqual(1, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type and metadata filtering (2)
            manager = new ExtensionsManager<ITestInterface, TestMetadata>();

            sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll", "Aliases=NotFound"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<ITestInterface, TestMetadata>>(manager);
            Assert.AreEqual(0, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type and metadata filtering (3)
            manager = new ExtensionsManager<ITestInterface, TestMetadata>();

            sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll", "Aliases=*"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<ITestInterface, TestMetadata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);
        }

        private void DumpExtensions<T>(T manager)
        {
            if (typeof(T).Equals(typeof(ExtensionsManager<ITestInterface>))){
                ExtensionsManager<ITestInterface> instance = (ExtensionsManager<ITestInterface>)Convert.ChangeType(manager, typeof(ExtensionsManager<ITestInterface>));

                foreach (Extension<ITestInterface> extension in instance.Extensions)
                {
                    Trace.WriteLine("Extension '" + extension.ValueType.ToString() + "' found");
                }
            }

            if (typeof(T).Equals(typeof(ExtensionsManager<ITestInterface, TestMetadata>)))
            {
                ExtensionsManager<ITestInterface, TestMetadata> instance = (ExtensionsManager<ITestInterface, TestMetadata>)Convert.ChangeType(manager, typeof(ExtensionsManager<ITestInterface, TestMetadata>));

                foreach (Extension<ITestInterface, TestMetadata> extension in instance.Extensions)
                {
                    Trace.WriteLine("Extension '" + extension.ValueType.ToString() + "' (Aliases '" + string.Join(",",extension.Metadata.Aliases) + "') found");
                }
            }
        }
    }
}
