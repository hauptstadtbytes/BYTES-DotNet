//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

//import namespace(s) required from 'BYTES.NET' library
using BYTES.NET.Collections.Registry;
using System.Diagnostics;

namespace BYTES.NET.TEST.Collections
{

    [TestClass]
    public class TestRegistryNode
    {

        [TestMethod]
        public void ParsePath()
        {

            //check for the local machine root path
            Node localMachine = new Node("HKEY_LOCAL_MACHINE");
            Microsoft.Win32.RegistryKey root = localMachine.Root;

            Assert.AreEqual(Microsoft.Win32.Registry.LocalMachine.Name, root.Name);

            //check for the software list
            localMachine = new Node("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            int count = localMachine.Children.Length;

            Microsoft.Win32.RegistryKey reference = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            int referenceCount = reference.GetSubKeyNames().Length;

            Assert.AreEqual(referenceCount, count);

        }

        [TestMethod]
        public void SearchChildNode()
        {

            //create the parent node manager instance
            Node software = new Node("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");

            //search for the 'Microsoft Access database engine'
            Dictionary<string, string> filter = new Dictionary<string, string>();
            filter.Add("DisplayName", "Microsoft Access database engine");

            List<Node.EnumerationOptions> options = new List<Node.EnumerationOptions>();
            options.Add(Node.EnumerationOptions.IgnoreCase);
            options.Add(Node.EnumerationOptions.ContainsSearch);

            Node[] result = software.SearchForChildren(filter, options.ToArray());

            Assert.AreEqual(1,result.Length);

            //write the display name to output
            Trace.Write("Application '" + result[0].Values["DisplayName"] + "' found");

        }

    }

}
