//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

//import namespace(s) required from 'BYTES.NET' library
using BYTES.NET.Primitives;


namespace BYTES.NET.TEST.Primitives
{

    [TestClass]
    public class TestStringExtensions
    {

        [TestMethod]
        public void GetTrigramSimilarity()
        {

            //create a new string value
            string theValue = "Phone";

            //validate the similarities for variing refence strings
            Assert.AreEqual(1, theValue.TrigramSimilarityTo("Phone"));
            Assert.IsTrue(0.5 > theValue.TrigramSimilarityTo("Postpone"));

        }

        [TestMethod]
        public void GetBestMatch()
        {

            //create a new string value
            string theValue = "Match";

            //get the best match
            string[] list = { "Hash", "Batch", "Mitch" };

            Assert.AreEqual("Batch", theValue.GetBestTrigramSimilarityMatch(list));

        }

    }

}
