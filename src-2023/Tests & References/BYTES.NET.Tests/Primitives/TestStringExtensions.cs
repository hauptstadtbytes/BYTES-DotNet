//import .net (default) namespace(s) required
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BYTES.NET.Tests.Primitives
{
    [TestClass]
    public class TestStringExtensions
    {
        [TestMethod]
        public void TestStringPatterns()
        {
            string toBeTested = "Hello_String";

            Regex rule = GetWildcardRegEx("Hello String");
            Assert.AreEqual(false, toBeTested.MatchesPattern(rule));

            rule = GetWildcardRegEx("Hello*");
            Assert.AreEqual(true, toBeTested.MatchesPattern(rule));

            rule = GetWildcardRegEx("*String");
            Assert.AreEqual(true, toBeTested.MatchesPattern(rule));

            rule = GetWildcardRegEx("Hello_String");
            Assert.AreEqual(true, toBeTested.MatchesPattern(rule));

            rule = GetWildcardRegEx("*ello*ring");
            Assert.AreEqual(true, toBeTested.MatchesPattern(rule));
        }

        [TestMethod]
        public void TestAllIndexesOf()
        {
            string inputString = "D:\\Das*\\ist\\das\\H*";
            char character = '*';

            int[] result = inputString.AllIndexesOf(character);
            Debug.WriteLine("Finding all indexes for '" + character.ToString() + "' in '" + inputString + "' resulted in '" + System.String.Join(",", result) + "'");
            Assert.AreEqual("6,17", System.String.Join(",", result));

            character = '\\';
            result = inputString.AllIndexesOf(character);
            Debug.WriteLine("Finding all indexes for '" + character.ToString() + "' in '" + inputString + "' resulted in '" + System.String.Join(",", result) + "'");
            Assert.AreEqual("2,7,11,15", System.String.Join(",", result));

            Regex expression = new Regex(@"(\\|\/)");
            result = inputString.AllIndexesOf(expression);
            Debug.WriteLine("Finding all indexes for '" + expression.ToString() + "' in '" + inputString + "' resulted in '" + System.String.Join(",", result) + "'");
            Assert.AreEqual("2,7,11,15", System.String.Join(",", result));

            expression = new Regex(@"\*");
            result = inputString.AllIndexesOf(expression);
            Debug.WriteLine("Finding all indexes for '" + expression.ToString() + "' in '" + inputString + "' resulted in '" + System.String.Join(",", result) + "'");
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
            Debug.WriteLine("Expanding variable(s) '" + System.String.Join(",", variables.Keys) + "' in '" + inputString + "' (ignoring cases) resulted in '" + result + "'");
            Assert.AreEqual("Hello World Example", result);

            //expanding the variable(s), respecting cases
            result = inputString.Expand(variables, false);
            Debug.WriteLine("Expanding variable(s) '" + System.String.Join(",", variables.Keys) + "' in '" + inputString + "' (respecting cases) resulted in '" + result + "'");
            Assert.AreEqual("Hello %World% Example", result);

        }

        [TestMethod]
        public void TestSimilarity()
        {
            //calculat the trigram-based similarities (as default algorithm)
            string theValue = "Phone";
            string reference = "Phone";

            double result = (double)theValue.SimilarityTo(reference);
            Debug.WriteLine("The similarity of '" + reference + "' to '" + theValue + "' is '" + result.ToString("G") + "'"); 
            Assert.AreEqual(1, result);

            reference = "Phones";
            result = (double)theValue.SimilarityTo(reference);
            Debug.WriteLine("The similarity of '" + reference + "' to '" + theValue + "' is '" + result.ToString("G") + "'");
            Assert.AreEqual(System.Convert.ToDouble(10) / System.Convert.ToDouble(15), result);

            reference = "Postpone";
            result = (double)theValue.SimilarityTo(reference);
            Debug.WriteLine("The similarity of '" + reference + "' to '" + theValue + "' is '" + result.ToString("G") + "'");
            Assert.IsTrue(0.5 > result);

            string[] options = new string[] { "Phone", "Phones", "Postpone" };
            Dictionary<string, double> resultDictionary = theValue.SimilarityTo(options);
            Debug.WriteLine("The similarity of '" + theValue + "' to '" + System.String.Join(",", options) + "' is '" + string.Join(",", resultDictionary.Select(x => x.Key + "=" + x.Value).ToArray()) + "'");
            Assert.AreEqual(1, resultDictionary["Phone"]);

            //calculat the levenshtein distance
            string str1 = "kitten";
            string str2 = "sitting";
            Assert.IsTrue(str1.SimilarityTo(str2, "levenshtein") < 0.25);

            str1 = "hello";
            str2 = "holla";
            Assert.IsTrue(str1.SimilarityTo(str2, "levenshtein") < 0.25);

            str1 = "short";
            str2 = "shortest";
            Assert.IsTrue(str1.SimilarityTo(str2, "levenshtein") < 0.25);

            str1 = "longer";
            str2 = "longest";
            Assert.IsTrue(str1.SimilarityTo(str2, "levenshtein") < 0.25);



        }

        [TestMethod]
        public void TestBestMatch()
        {
            //get the best match, based on the trigram-based similarities
            string theValue = "Phone";
            string[] options = new string[] { "Phones", "Postpone" };
            double threshold = 0.65;
            string? match = theValue.BestMatch(options,"trigram", threshold);
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' (using a threshold of '" + threshold.ToString() + "') is '" + match + "'");
            Assert.AreEqual("Phones", match);

            threshold = 0.70;
            match = theValue.BestMatch(options,"trigram",threshold);
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' (using a threshold of '" + threshold.ToString() + "') is '" + match + "'");
            Assert.AreEqual(null, match); //the output is not matching the threshold given

            theValue = "Match";
            options = new string[] { "Hash", "Batch", "Mitch" };
            match = theValue.BestMatch(options); //no threshold is given
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "'");
            Assert.AreEqual("Batch", match);

            //get the best match, based on levenshtein
            options = new string[] { "Phones", "Postpone" };
            match = theValue.BestMatch(options, "levenshtein");
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "'");
            Assert.AreEqual("Phones", match);

            theValue = "Match";
            options = new string[] { "Hash", "Batch", "Mitch" };
            string? resultString = theValue.BestMatch(options, "levenshtein");
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + resultString + "'");
            //Assert.AreEqual("Batch", resultString);
        }

        private Regex GetWildcardRegEx(string input)
        {
            input = input.Replace("*", "[\\w|\\W]*");
            return new Regex("^" + input + "$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
    }

}
