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
            result = (double)str1.SimilarityTo(str2, "levenshtein");
            Debug.WriteLine("The similarity of '" + str1 + "' to '" + str2 + "' is '" + result.ToString("G") + "'");
            Assert.IsTrue(result < 0.47);

            str1 = "hello";
            str2 = "holla";
            result = (double)str1.SimilarityTo(str2, "levenshtein");
            Debug.WriteLine("The similarity of '" + str1 + "' to '" + str2 + "' is '" + result.ToString("G") + "'");
            Assert.IsTrue(result < 0.41);
        }

        [TestMethod]
        public void TestBestMatch()
        {
            //get the best match, based on the trigram-based similarities
            string theValue = "Phone";
            string[] options = new string[] { "Phones", "Postpone" };
            double threshold = 0.65;
            double dist = 0;
            string? match = theValue.BestMatch(options, out dist, "trigram", threshold);
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' (using a threshold of '" + threshold.ToString() + "') is '" + match + "' with a simliarity of " + dist);
            Assert.AreEqual("Phones", match);

            threshold = 0.70;
            match = theValue.BestMatch(options, out dist, "trigram",threshold);
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' (using a threshold of '" + threshold.ToString() + "') is '" + match + "' with a simliarity of " + dist);
            Assert.AreEqual(null, match); //the output is not matching the threshold given

            theValue = "Match";
            options = new string[] { "Hash", "Batch", "Mitch" };
            match = theValue.BestMatch(options, out dist, "trigram"); //no threshold is given
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "' with a simliarity of " + dist);
            Assert.AreEqual("Batch", match);

            //get the best match, based on levenshtein
            theValue = "Phone";
            options = new string[] { "Phones", "Postpone" };
            match = theValue.BestMatch(options, out dist, "trigram"); //no threshold is given
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "' with a simliarity of " + dist);
            Assert.AreEqual("Phones", match);

            theValue = "Match";
            options = new string[] { "Hash", "Batch", "Mitch" };
            string? resultString = theValue.BestMatch(options, out dist, "levenshtein");
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + resultString + "' with a distance of " + dist);
            Assert.AreEqual("Batch", resultString);

            theValue = "Meier";
            options = new string[] { "Maier", "Mayer", "Meyer" };
            match = theValue.BestMatch(options, "levenshtein");
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "' ");
            Assert.AreEqual("Maier", match);

            theValue = "Phone";
            options = new string[] { "Phones", "Telefon", "Telephone", "Mobile Phone" };
            match = theValue.BestMatch(options, out dist, "levenshtein");
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "' with a distance of " + dist);
            Assert.AreEqual("Phones", match);

            theValue = "Mobile";
            options = new string[] { "Phones", "Telefon", "Telephone", "Mobile Phone" };
            match = theValue.BestMatch(options, out dist, "levenshtein");
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "' with a distance of " + dist);
            Assert.AreEqual("Mobile Phone", match);

            theValue = "München";
            options = new string[] { "Muenchen", "Munic", "München" };
            match = theValue.BestMatch(options, out dist, "levenshtein");
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "' with a distance of " + dist);
            Assert.AreEqual("München", match);

            theValue = "München";
            options = new string[] { "Muenchen", "Munic"};
            match = theValue.BestMatch(options, out dist, "levenshtein");
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "' with a distance of " + dist);
            Assert.AreEqual("Muenchen", match);

            theValue = "Hallo";
            options = new string[] { "Welt", "Auto", "Franz", "Garten" };
            match = theValue.BestMatch(options, out dist, "levenshtein");
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "' with a distance of " + dist);
            Assert.AreEqual("Welt", match);

            theValue = "Hallo";
            options = new string[] { "Welt", "Auto", "Franz", "Garten" };
            match = theValue.BestMatch(options, out dist, "levenshtein", 0.5);
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "' with a distance of " + dist);
            Assert.AreEqual(null, match);
        }

        [TestMethod]
        public void CompareSpeedBestMatch()
        {
            double dist = 0;
            //See the faster Algorythm for a normal string
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string theValue = "Phone";
            string[] options = new string[] { "Phones", "Postpone" };
            string? match = theValue.BestMatch(options, out dist, "trigram");
            watch.Stop();
            var elapsedMsTrigram = watch.ElapsedMilliseconds;
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + " is '" + match + "'");

            watch = System.Diagnostics.Stopwatch.StartNew();
            theValue = "Phone";
            options = new string[] { "Phones", "Postpone" };
            match = theValue.BestMatch(options, out dist, "levenshtein");
            watch.Stop();
            var elapsedMsLevenhstein = watch.ElapsedMilliseconds;
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "'");

            if (elapsedMsTrigram < elapsedMsLevenhstein)
            {
                Debug.WriteLine("Trigram was faster than Levenhstein");
            }
            if (elapsedMsTrigram > elapsedMsLevenhstein)
            {
                Debug.WriteLine("Levenhstein was faster than Trigram");
            }
            else
            {
                Debug.WriteLine("Trigram and Levenhstein have the same speed");
            }

            //See the faster Algorythm for a short string
            watch = System.Diagnostics.Stopwatch.StartNew();
            theValue = "Ab";
            options = new string[] { "Ab", "Abb" };
            match = theValue.BestMatch(options, out dist, "trigram");
            watch.Stop();
            elapsedMsTrigram = watch.ElapsedMilliseconds;
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + " is '" + match + "'");

            watch = System.Diagnostics.Stopwatch.StartNew();
            theValue = "Ab";
            options = new string[] { "Ab", "Abb" };
            match = theValue.BestMatch(options, out dist, "levenshtein");
            watch.Stop();
            elapsedMsLevenhstein = watch.ElapsedMilliseconds;
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "'");

            if (elapsedMsTrigram < elapsedMsLevenhstein)
            {
                Debug.WriteLine("Trigram was faster than Levenhstein");
            }
            if (elapsedMsTrigram > elapsedMsLevenhstein)
            {
                Debug.WriteLine("Levenhstein was faster than Trigram");
            }
            else
            {
                Debug.WriteLine("Trigram and Levenhstein have the same speed");
            }

            //See the faster Algorythm for a long string
            watch = System.Diagnostics.Stopwatch.StartNew();
            theValue = "Despite the persistent drizzle, the determined runner, who wore a bright yellow raincoat and a pair of sturdy running shoes, continued her morning jog along the scenic, tree-lined path, where the chirping birds and the rustling leaves created a symphony of nature, and she greeted the occasional passerby with a cheerful wave, while maintaining her steady pace and focusing on her breathing technique";
            options = new string[] { "Despite persistent drizzle, the determined runner, who wore a bright yellow raincoat and a pair of sturdy running shoes, continued her morning jog along the scenic, tree-lined path, where the chirping birds and the rustling leaves created a symphony of nature, and she greeted the occasional passerby with a cheerful wave, while maintaining her steady pace and focusing on her breathing technique", "Despite the persistent drizzle, the determined runner, who wore a bright yellow raincoat and a pair of sturdy running shoes, continued her morning jog along the scenic, tree-lined path, where the chirping birds and the rustling leaves created a symphony of nature, and she greeted the occasional passerby with a cheerful wave, while maintaining her steady pace and focusing on her breathing technique" };
            match = theValue.BestMatch(options, out dist, "trigram");
            watch.Stop();
            elapsedMsTrigram = watch.ElapsedMilliseconds;
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + " is '" + match + "'");

            watch = System.Diagnostics.Stopwatch.StartNew();
            theValue = "Despite the persistent drizzle, the determined runner, who wore a bright yellow raincoat and a pair of sturdy running shoes, continued her morning jog along the scenic, tree-lined path, where the chirping birds and the rustling leaves created a symphony of nature, and she greeted the occasional passerby with a cheerful wave, while maintaining her steady pace and focusing on her breathing technique";
            options = new string[] { "Despite persistent drizzle, the determined runner, who wore a bright yellow raincoat and a pair of sturdy running shoes, continued her morning jog along the scenic, tree-lined path, where the chirping birds and the rustling leaves created a symphony of nature, and she greeted the occasional passerby with a cheerful wave, while maintaining her steady pace and focusing on her breathing technique", "Despite the persistent drizzle, the determined runner, who wore a bright yellow raincoat and a pair of sturdy running shoes, continued her morning jog along the scenic, tree-lined path, where the chirping birds and the rustling leaves created a symphony of nature, and she greeted the occasional passerby with a cheerful wave, while maintaining her steady pace and focusing on her breathing technique" };
            match = theValue.BestMatch(options, out dist, "levenshtein");
            watch.Stop();
            elapsedMsLevenhstein = watch.ElapsedMilliseconds;
            Debug.WriteLine("The best match of '" + System.String.Join(",", options) + "' for '" + theValue + "' is '" + match + "'");

            if (elapsedMsTrigram < elapsedMsLevenhstein)
            {
                Debug.WriteLine("Trigram was faster than Levenhstein");
            }
            if (elapsedMsTrigram > elapsedMsLevenhstein)
            {
                Debug.WriteLine("Levenhstein was faster than Trigram");
            }
            else
            {
                Debug.WriteLine("Trigram and Levenhstein have the same speed");
            }
        }

        private Regex GetWildcardRegEx(string input)
        {
            input = input.Replace("*", "[\\w|\\W]*");
            return new Regex("^" + input + "$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
    }

}
