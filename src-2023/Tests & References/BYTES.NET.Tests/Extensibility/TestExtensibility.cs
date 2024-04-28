//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Extensibility;

//import internal namespace(s) required
using BYTES.NET.Tests.Extensibility.API;
using BYTES.NET.Extensibility.API;
using BYTES.NET.Logging;

namespace BYTES.NET.Tests.Extensibility
{
    [TestClass]
    public class TestExtensibility
    {
        [TestMethod]
        public void TestSimpleEnumeration()
        {
            //enumerate, using the file path 'String' type
            ExtensionsManager<ISampleInterface> manager = new ExtensionsManager<ISampleInterface>();
            manager.Update(new string[] { "%BYTES.NET.DIR%\\BYTES.NET.Tests.dll" });

            DumpExtensions<ExtensionsManager<ISampleInterface>>(manager);
            Assert.AreEqual(3, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type
            manager = new ExtensionsManager<ISampleInterface>();

            List<FileSystemExtensionsSource> sources = new List<FileSystemExtensionsSource>();
            sources.Add(new FileSystemExtensionsSource("%BYTES.NET%"));
            sources.Add(new FileSystemExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Tests.dll"));

            manager.Update(sources.ToArray());

            Debug.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<ISampleInterface>>(manager);
            Assert.AreEqual(3, manager.Extensions.Length);
        }

        [TestMethod]
        public void TestAdvancedEnumeration()
        {
            //enumerate, using the file path 'String' type
            ExtensionsManager<ISampleInterface, SampleMetadata> manager = new ExtensionsManager<ISampleInterface, SampleMetadata>();
            manager.Update(new string[] { "%BYTES.NET.DIR%\\BYTES.NET.Tests.dll" });

            DumpExtensions<ExtensionsManager<ISampleInterface, SampleMetadata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type (1)
            manager = new ExtensionsManager<ISampleInterface, SampleMetadata>();

            List<FileSystemExtensionsSource> sources = new List<FileSystemExtensionsSource>();
            sources.Add(new FileSystemExtensionsSource("%BYTES.NET%"));
            sources.Add(new FileSystemExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Tests.dll"));

            manager.Update(sources.ToArray());

            Debug.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<ISampleInterface, SampleMetadata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type and metadata filtering (1)
            manager = new ExtensionsManager<ISampleInterface, SampleMetadata>();

            sources = new List<FileSystemExtensionsSource>();
            sources.Add(new FileSystemExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Tests.dll", "Name=TestImplementationTwo"));

            manager.Update(sources.ToArray());

            Debug.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<ISampleInterface, SampleMetadata>>(manager);
            Assert.AreEqual(1, manager.Extensions.Length);

            Assert.AreEqual("Hello World!", manager.Extensions[0].Value().Transform("Any Text"));

            //enumerate, using the 'ExtensionsSource' type and metadata filtering (2)
            manager = new ExtensionsManager<ISampleInterface, SampleMetadata>();

            sources = new List<FileSystemExtensionsSource>();
            sources.Add(new FileSystemExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Tests.dll", "Aliases=NotFound"));

            manager.Update(sources.ToArray());

            Debug.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<ISampleInterface, SampleMetadata>>(manager);
            Assert.AreEqual(0, manager.Extensions.Length);

            //enumerate, using the 'ExtensionsSource' type and metadata filtering (3)
            manager = new ExtensionsManager<ISampleInterface, SampleMetadata>();

            sources = new List<FileSystemExtensionsSource>();
            FileSystemExtensionsSource source = new FileSystemExtensionsSource("%BYTES.NET.DIR%\\BYTES.NET.Tests.dll", "Aliases=*");
            sources.Add(source);

            manager.Update(sources.ToArray());
            //DumpLog(source.Log);

            Debug.WriteLine(String.Empty);
            DumpExtensions<ExtensionsManager<ISampleInterface, SampleMetadata>>(manager);
            Assert.AreEqual(2, manager.Extensions.Length);
        }

        private void DumpExtensions<T>(T manager)
        {
            if (typeof(T).Equals(typeof(ExtensionsManager<ISampleInterface>)))
            {
                ExtensionsManager<ISampleInterface> instance = (ExtensionsManager<ISampleInterface>)Convert.ChangeType(manager, typeof(ExtensionsManager<ISampleInterface>));

                foreach (Extension<ISampleInterface> extension in instance.Extensions)
                {
                    Debug.WriteLine("Extension '" + extension.ValueType.ToString() + "' found");
                }
            }

            if (typeof(T).Equals(typeof(ExtensionsManager<ISampleInterface, SampleMetadata>)))
            {
                ExtensionsManager<ISampleInterface, SampleMetadata> instance = (ExtensionsManager<ISampleInterface, SampleMetadata>)Convert.ChangeType(manager, typeof(ExtensionsManager<ISampleInterface, SampleMetadata>));

                foreach (Extension<ISampleInterface, SampleMetadata> extension in instance.Extensions)
                {
                    Debug.WriteLine("Extension '" + extension.ValueType.ToString() + "' (Aliases '" + string.Join(",", extension.Metadata.Aliases) + "') found");
                }
            }
        }

        private void DumpLog(Log log)
        {
            Debug.WriteLine("Data from log '" + log.ID + "'");

            foreach(LogEntry entry in log.GetCache())
            {
                Debug.WriteLine(entry.ToString());
            }

            Debug.WriteLine(String.Empty);
        }
    }
}
