//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

//import .net namespace(s) required
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.Test.Primitives
{
    [TestClass]
    public class TestStringExtensions
    {
        [TestMethod]
        public void TestAllIndexesOf()
        {

            string inputString = "D:\\Das*\\ist\\das\\H*";
            char character = '*';

            int[] result = inputString.AllIndexesOf(character);
            Trace.WriteLine("Finding all indexes for '" + character.ToString() + "' in '" + inputString + "' resulted in '" + System.String.Join(",", result) + "'");
            Assert.AreEqual("6,17", System.String.Join(",", result));

            character = '\\';
            result = inputString.AllIndexesOf(character);
            Trace.WriteLine("Finding all indexes for '" + character.ToString() + "' in '" + inputString + "' resulted in '" + System.String.Join(",", result) + "'");
            Assert.AreEqual("2,7,11,15", System.String.Join(",", result));

            Regex expression = new Regex(@"(\\|\/)");
            result = inputString.AllIndexesOf(expression);
            Trace.WriteLine("Finding all indexes for '" + expression.ToString() + "' in '" + inputString + "' resulted in '" + System.String.Join(",", result) + "'");
            Assert.AreEqual("2,7,11,15", System.String.Join(",", result));

            expression = new Regex(@"\*");
            result = inputString.AllIndexesOf(expression);
            Trace.WriteLine("Finding all indexes for '" + expression.ToString() + "' in '" + inputString + "' resulted in '" + System.String.Join(",", result) + "'");
            Assert.AreEqual("6,17", System.String.Join(",", result));

        }

        [TestMethod]
        public void TestKeyValueParsing()
        {
            string inputString = "Hello:World";
            KeyValuePair<string, string> result = inputString.ParseKeyValue();
            Assert.AreEqual("Hello World", result.Key + " " + result.Value);

            inputString = "Path:\"C:\"myPath\"";
            result = inputString.ParseKeyValue();
            Assert.AreEqual("Path", result.Key);
            Assert.AreEqual("\"C:\"myPath\"", result.Value);

            inputString = "Key,Val*";
            result = inputString.ParseKeyValue(new char[] { ',' });
            Assert.AreEqual("Key", result.Key);
            Assert.AreEqual("Val*", result.Value);
        }

        [TestMethod]
        public void TestExpandVariables()
        {

            string inputString = "%Hello% %World% Example";
            Dictionary<string, string> variables = new Dictionary<string, string>()
            {
                {"%Hello%","Hello" },
                {"world", "World" }
            };

            //do a basic test (ignoring case)
            string result = inputString.Expand(variables);
            Trace.WriteLine("Expanding variable(s) '" + System.String.Join(",", variables.Keys) + "' in '" + inputString + "' (ignoring cases) resulted in '" + result + "'");
            Assert.AreEqual("Hello World Example", result);

            //expanding the variable(s), respecting cases
            result = inputString.Expand(variables, false);
            Trace.WriteLine("Expanding variable(s) '" + System.String.Join(",", variables.Keys) + "' in '" + inputString + "' (respecting cases) resulted in '" + result + "'");
            Assert.AreEqual("Hello %World% Example", result);

        }

        [TestMethod]
        public void TestSimilarityAndBestMatch()
        {

            string theValue = "Phone";
            string reference = "Phone";

            //do a basic test, calculating the similarities
            double result = theValue.SimilarityTo(reference);
            Trace.WriteLine("The similarity of '" + reference + "' to '" + theValue + "' is '" + result.ToString("G") + "'");
            Assert.AreEqual(1, result);

            reference = "Phones";
            result = theValue.SimilarityTo(reference);
            Trace.WriteLine("The similarity of '" + reference + "' to '" + theValue + "' is '" + result.ToString("G") + "'");
            Assert.AreEqual(System.Convert.ToDouble(10) / System.Convert.ToDouble(15), result);

            reference = "Postpone";
            result = theValue.SimilarityTo(reference);
            Trace.WriteLine("The similarity of '" + reference + "' to '" + theValue + "' is '" + result.ToString("G") + "'");
            Assert.IsTrue(0.5 > result);

            string[] options = new string[] { "Phone", "Phones", "Postpone" };
            Dictionary<string, double> resultDictionary = theValue.SimilarityTo(options);
            Trace.WriteLine("The similarity of '" + theValue + "' to '" + System.String.Join(",", options) + "' is '" + string.Join(",", resultDictionary.Select(x => x.Key + "=" + x.Value).ToArray()) + "'");
            Assert.AreEqual(1, resultDictionary["Phone"]);

            //get the best match
            options = new string[] { "Phones", "Postpone" };
            double threshold = 0.65;
            string resultString = theValue.GetBestMatch(options, threshold);
            Trace.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' (using a threshold of '" + threshold.ToString() + "') is '" + resultString + "'");
            Assert.AreEqual("Phones", resultString);

            threshold = 0.70;
            resultString = theValue.GetBestMatch(options, threshold);
            Trace.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' (using a threshold of '" + threshold.ToString() + "') is '" + resultString + "'");
            Assert.AreEqual(System.String.Empty, resultString);

            theValue = "Match";
            options = new string[] { "Hash", "Batch", "Mitch" };
            resultString = theValue.GetBestMatch(options);
            Trace.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + resultString + "'");
            Assert.AreEqual("Batch", resultString);

        }

        [TestMethod]
        public void TestFilePathParsing()
        {
            string theText = "%BYTES.NET.DIR%\\..\\..\\..\\..\\test\\CLI_SampleSettings.xml\\";
            Assert.AreEqual(false, theText.IsFilePath());

            theText = "%BYTES.NET.DIR%\\..\\..\\..\\..\\test\\CLI_SampleSettings.xml";
            Assert.AreEqual(true, theText.IsFilePath());

            theText = "D:\\MightBeAFile.docx";
            Assert.AreEqual(true, theText.IsFilePath());

            theText = "D:\\MightBeAFolder";
            Assert.AreEqual(false, theText.IsFilePath());
        }

    }
}
