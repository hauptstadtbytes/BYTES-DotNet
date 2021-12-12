//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Collections;

namespace BYTES.NET.Test.Collections
{

    [TestClass]
    public class TestArgumentsCollection
    {

        [TestMethod]
        public void TestNamedArgsParsing()
        {
            string[] testArgs = { "myMethod", "-path", "\"C:\\Sample\"", "/switch", "-key=value", "-anotherKey:AnotherValue", "/secondPath:\"C:\\Test\\2\""};

            ArgumentsCollection testCollection = new ArgumentsCollection(testArgs);

            Assert.AreEqual(5, testCollection.Names.Length);

            Assert.AreEqual(string.Empty, testCollection["switch"]);
            Assert.AreEqual(true,testCollection.ContainsArgument("switch"));

            Assert.AreEqual("value", testCollection["key"]);
            Assert.AreEqual("AnotherValue", testCollection["anotherKey"]);

            Assert.AreEqual("C:\\Sample", testCollection["path"]);
            Assert.AreEqual("C:\\Test\\2", testCollection["secondPath"]);

            Assert.AreEqual(false, testCollection.IsNamedArgument(0));

        }

    }
}
