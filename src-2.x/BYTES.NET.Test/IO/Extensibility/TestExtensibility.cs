//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

//import .net namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Extensibility;
using BYTES.NET.IO.Scripting;
using BYTES.NET.IO.Scripting.API;

namespace BYTES.NET.Test.IO.Extensibility
{
    [TestClass]
    public class TestExtensibility
    {
        [TestMethod]
        public void TestSimpleEnumeration()
        {
            //enumerate, using the file path 'String' type
            ExtensionsManager<IMethod> manager = new ExtensionsManager<IMethod>();
            manager.Update(new string[] {"%BYTES.NET%"});

            DumpExtensions<ExtensionsManager<IMethod>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type
            manager = new ExtensionsManager<IMethod>();
            
            List<ExtensionsSource> sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET%"));
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<IMethod>>(manager);
            Assert.AreEqual(3, manager.Extensions.Length);
        }

        [TestMethod]
        public void TestAdvancedEnumeration()
        {
            //enumerate, using the file path 'String' type
            ExtensionsManager<IMethod,MethodMetdata> manager = new ExtensionsManager<IMethod, MethodMetdata>();
            manager.Update(new string[] { "%BYTES.NET%" });

            DumpExtensions<ExtensionsManager<IMethod,MethodMetdata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type (1)
            manager = new ExtensionsManager<IMethod, MethodMetdata>();

            List<ExtensionsSource> sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET%"));
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<IMethod, MethodMetdata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type (2)
            manager = new ExtensionsManager<IMethod, MethodMetdata>();

            sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET%|BYTES.NET.DIR%\\BYTES.NET.Test.dll"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<IMethod, MethodMetdata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type and metadata filtering (1)
            manager = new ExtensionsManager<IMethod, MethodMetdata>();

            sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET%", "Name=SetVariable"));
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<IMethod, MethodMetdata>>(manager);
            Assert.AreEqual(1, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type and metadata filtering (2)
            manager = new ExtensionsManager<IMethod, MethodMetdata>();

            sources = new List<ExtensionsSource>();
            sources.Add(new ExtensionsSource("%BYTES.NET%", "Aliases=Log|*"));
            sources.Add(new ExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Test.dll"));

            manager.Update(sources.ToArray());

            Trace.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<IMethod, MethodMetdata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);
        }

        private void DumpExtensions<T>(T manager)
        {
            if (typeof(T).Equals(typeof(ExtensionsManager<IMethod>))){
                ExtensionsManager<IMethod> instance = (ExtensionsManager<IMethod>)Convert.ChangeType(manager, typeof(ExtensionsManager<IMethod>));

                foreach (Extension<IMethod> extension in instance.Extensions)
                {
                    Trace.WriteLine("Extension '" + extension.ValueType.ToString() + "' found");
                }
            }

            if (typeof(T).Equals(typeof(ExtensionsManager<IMethod,MethodMetdata>)))
            {
                ExtensionsManager<IMethod, MethodMetdata> instance = (ExtensionsManager<IMethod, MethodMetdata>)Convert.ChangeType(manager, typeof(ExtensionsManager<IMethod, MethodMetdata>));

                foreach (Extension<IMethod, MethodMetdata> extension in instance.Extensions)
                {
                    Trace.WriteLine("Extension '" + extension.ValueType.ToString() + "' (Aliases '" + extension.Metadata.Aliases + "') found");
                }
            }
        }
    }
}
