//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

//import .net namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO;
using BYTES.NET.IO.Extensibility;

namespace BYTES.NET.Test.IO.Extensibility
{
    [TestClass]
    public class TestExtensibility
    {
        [TestMethod]
        public void TestSimpleEnumeration()
        {
            //enumerate, using the file path 'String' type
            ExtensionsManager<Scripting.API.ITestMethod> manager = new ExtensionsManager<Scripting.API.ITestMethod>();
            manager.Update(new string[] {"%BYTES.NET%"});

            DumpExtensions<ExtensionsManager<Scripting.API.ITestMethod>>(manager);
            Assert.AreEqual(0, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type
            manager = new ExtensionsManager<Scripting.API.ITestMethod>();
            
            List<ExtensionsSource> sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET%"));
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<Scripting.API.ITestMethod>>(manager);
            Assert.AreEqual(3, manager.Extensions.Length);
        }

        [TestMethod]
        public void TestAdvancedEnumeration()
        {
            //enumerate, using the file path 'String' type
            ExtensionsManager<Scripting.API.ITestMethod, Metadata> manager = new ExtensionsManager<Scripting.API.ITestMethod, Metadata>();
            manager.Update(new string[] { "%BYTES.NET%" });

            DumpExtensions<ExtensionsManager<Scripting.API.ITestMethod, Metadata>>(manager);
            Assert.AreEqual(0, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type (1)
            manager = new ExtensionsManager<Scripting.API.ITestMethod, Metadata>();

            List<ExtensionsSource> sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET%"));
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<Scripting.API.ITestMethod, Metadata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type and metadata filtering (1)
            manager = new ExtensionsManager<Scripting.API.ITestMethod, Metadata>();

            sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll", "Name=SetVariable"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<Scripting.API.ITestMethod, Metadata>>(manager);
            Assert.AreEqual(1, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type and metadata filtering (2)
            manager = new ExtensionsManager<Scripting.API.ITestMethod, Metadata>();

            sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll", "Aliases=Log|*"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<Scripting.API.ITestMethod, Metadata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);
        }

        private void DumpExtensions<T>(T manager)
        {
            if (typeof(T).Equals(typeof(ExtensionsManager<Scripting.API.ITestMethod>))){
                ExtensionsManager<Scripting.API.ITestMethod> instance = (ExtensionsManager<Scripting.API.ITestMethod>)Convert.ChangeType(manager, typeof(ExtensionsManager<Scripting.API.ITestMethod>));

                foreach (Extension<Scripting.API.ITestMethod> extension in instance.Extensions)
                {
                    Trace.WriteLine("Extension '" + extension.ValueType.ToString() + "' found");
                }
            }

            if (typeof(T).Equals(typeof(ExtensionsManager<Scripting.API.ITestMethod, Metadata>)))
            {
                ExtensionsManager<Scripting.API.ITestMethod, Metadata> instance = (ExtensionsManager<Scripting.API.ITestMethod, Metadata>)Convert.ChangeType(manager, typeof(ExtensionsManager<Scripting.API.ITestMethod, Metadata>));

                foreach (Extension<Scripting.API.ITestMethod, Metadata> extension in instance.Extensions)
                {
                    Trace.WriteLine("Extension '" + extension.ValueType.ToString() + "' (Aliases '" + extension.Metadata.Aliases + "') found");
                }
            }
        }
    }
}
