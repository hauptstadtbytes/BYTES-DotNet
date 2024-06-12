//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives
{
    /// <summary>
    /// string type extension method(s)
    /// </summary>
    public static class StringExtensions
    {

        #region public method(s)

#if NETFULL

        /// <summary>
        /// removes all leading or trailing instances of a character from the string given
        /// </summary>
        /// <param name="text"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        /// <remarks>a polymorph implementation for .NET 4.6/ 4.8</remarks>
        public static string Trim(this string text, char character)
        {

            string output = text;

            while (output.StartsWith(character.ToString()))
            {
                output = output.Substring(1);
            }

            while (output.EndsWith(character.ToString()))
            {
                output = output.Substring(0, output.Length - 2);
            }

            return output;

        }
#endif

        /// <summary>
        /// returns a (zero-based) list of all indexes of a character inside a string given
        /// </summary>
        /// <param name="str"></param>
        /// <param name="character"></param>
        /// <remarks>based on an article found at 'https://stackoverflow.com/questions/2641326/finding-all-positions-of-substring-in-a-larger-string-in-c-sharp', ignoring overlapping occurences</remarks>
        public static int[] AllIndexesOf(this string str, char character)
        {

            List<int> output = new List<int>();

            for (int index = 0; ; index += 1)
            {

                index = str.IndexOf(character, index);

                if (index == -1)
                {
                    break;
                }

                output.Add(index);

            }

            return output.ToArray();

        }

        /// <summary>
        /// returns a (zero-based) list of all indexes of strings matching the expression given
        /// </summary>
        /// <param name="str"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int[] AllIndexesOf(this string str, Regex expression)
        {

            List<int> output = new List<int>();

            foreach (Match match in expression.Matches(str))
            {
                output.Add(match.Index);
            }

            return output.ToArray();

        }

        /// <summary>
        /// checks (optional case in-sensitive) if a strings type array contains a specific value
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="match"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool Contains(this string[] strings, string match, bool ignoreCase = true)
        {

            foreach (string str in strings)
            {

                if (ignoreCase)
                {
                    if (str.ToLower() == match.ToLower())
                    {
                        return true;
                    }
                }
                else
                {
                    if (str == match)
                    {
                        return true;
                    }
                }

            }

            return false;

        }

        /// <summary>
        /// expands variables inside a string given
        /// </summary>
        /// <param name="text"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string Expand(this System.String text, Dictionary<string, string> variables, bool ignoreCase = true)
        {

            string output = text;

            foreach (KeyValuePair<string, string> pair in variables)
            {
                //parse the key
                string key = pair.Key;

                if (!key.StartsWith("%"))
                {
                    key = "%" + key;
                }
                if (!key.EndsWith("%"))
                {
                    key = key + "%";
                }

                //setup the regular expression
                Regex myRegex = new Regex(key, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

                if (!ignoreCase)
                {
                    myRegex = new Regex(key, RegexOptions.CultureInvariant);
                }

                //replace the mask(s)
                output = myRegex.Replace(output, pair.Value);

            }

            return output;

        }

        /// <summary>
        /// eases RegEx-based matching for string type value(s)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pattern"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool MatchesPattern(this string text, Regex pattern, out Match? match)
        {
            foreach (Match myMatch in pattern.Matches(text))
            {
                if (myMatch.Success)
                {
                    match = myMatch;
                    return true;
                }
            }

            match = null;
            return false;
        }

        /// <summary>
        /// eases RegEx-based matching for string type value(s)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        /// <remarks>returns no 'Match' typ out</remarks>
        public static bool MatchesPattern(this string text, Regex pattern)
        {
            return MatchesPattern(text, pattern, out _);
        }

        /// <summary>
        /// returns a key-value-pair from a string containing an equality character
        /// </summary>
        /// <param name="text"></param>
        /// <param name="equalitySigns"></param>
        /// <returns></returns>
        /// <remarks>if an equality sign occures multiple times or there are multiple equality signs, only the left-first one is taken into account</remarks>
        public static KeyValuePair<string, string> ParseKeyValue(this string text, char[] equalitySigns = null)
        {

            //parse the arguments
            if (equalitySigns == null || equalitySigns.Length == 0)
            {
                equalitySigns = new char[] { '=', ':' };
            }

            //assemble the regular expression
            string expression = @"\s*[";
            int counter = 0;

            foreach (char sign in equalitySigns)
            {
                counter += 1;

                if (counter > 1)
                {
                    expression += "|";
                }

                expression += sign;

            }

            expression += @"]\s*";

            Regex myRegex = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            //get a list of all indexes
            int[] indexes = text.AllIndexesOf(myRegex);

            //return the output value
            if (indexes.Length == 0)
            {
                return new KeyValuePair<string, string>(text, null);
            }

            return new KeyValuePair<string, string>(text.Substring(0, indexes[0]), text.Substring(indexes[0] + 1));

        }

        /// <summary>
        /// returns the string similarity to a string given
        /// </summary>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <param name="algorithm">the algorithm used; options are 'trigram' and 'levenshtein'</param>
        /// <returns></returns>
        public static double? SimilarityTo(this System.String text, string reference, string algorithm = "trigram")
        {
            switch(algorithm.ToLower())
            {
                case "trigram":
                    return TrigramSimilarityTo(text,reference);
                case "levenshtein":
                    return LevenshteinDistanceNormalizedTo(text,reference);
                default:
                    return null;
            }
        }

        /// <summary>
        /// returns the string similarity for an array given
        /// </summary>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <param name="algorithm">the algorithm used; options are 'trigram' and 'levenshtein'</param>
        /// <returns></returns>
        public static Dictionary<string, double> SimilarityTo(this System.String text, IEnumerable<string> reference, string algorithm = "trigram")
        {

            Dictionary<string, double> output = new Dictionary<string, double>();

            foreach (string referenceString in reference)
            {
                double? similarity = text.SimilarityTo(referenceString, algorithm);

                if(similarity != null)
                {
                    output.Add(referenceString, (double)similarity);
                }
                
            }

            return output;

        }

        /// <summary>
        /// returns the best match from a list of options given (respecting an optional threshold)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="options"></param>
        /// <param name="algorithm">the algorithm used; options are 'trigram' and 'levenshtein'</param>
        /// <param name="threshold"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        public static string? BestMatch(this System.String text, IEnumerable<string> options, out double dist, string algorithm = "trigram", double? threshold = null)
        {
            switch (algorithm.ToLower())
            {
                case "trigram":
                    if(threshold == null)
                    {
                        threshold = 0;
                    }
                    return GetBestMatchUsingTrigrams(text, options, out dist, (double)threshold);
                case "levenshtein":
                    return GetBestMatchUsingLevenshteinDistanceNormalized(text, options, out dist, threshold);
                default:
                    dist = 0;
                    return null;
            }
        }
        public static string? BestMatch(this System.String text, IEnumerable<string> options, string algorithm = "trigram", double? threshold = null)
        {
            double result = 0;

            return BestMatch(text, options, out result, algorithm, threshold);
        }

        #endregion

        #region private method(s) supporting trigram-based similarity

        /// <summary>
        /// caldulates all trigrams from a string given
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string[] GetTrigrams(string source)
        {

            List<string> output = new List<string>();

            for (int i = 0; i <= source.Length + 1; i++)
            {

                int index = i - 2;
                string trigram = string.Empty;

                switch (index)
                {
                    case -2:
                        trigram = "  " + source.Substring(0, 1).ToUpper();
                        break;

                    case -1:
                        trigram = " " + source.Substring(0, 2).ToUpper();
                        break;

                    default:
                        if (index == (source.Length - 2))
                        {
                            trigram = source.Substring(index, 2).ToUpper() + " ";
                            break;
                        }
                        else if (index == (source.Length - 1))
                        {
                            trigram = source.Substring(index, 1).ToUpper() + "  ";
                            break;
                        }
                        else
                        {
                            trigram = source.Substring(index, 3).ToUpper();
                            break;
                        }

                }

                output.Add(trigram);

            }

            return output.ToArray();

        }

        /// <summary>
        /// calculates the trigam-based similarity
        /// </summary>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        private static double TrigramSimilarityTo(string text, string reference)
        {

            string[] inputTrigrams = GetTrigrams(text);
            string[] referenceTrigrams = GetTrigrams(reference);

            int counter = 0;
            foreach (string trigram in referenceTrigrams)
            {

                if (inputTrigrams.Contains(trigram))
                {
                    counter += 1;
                }

            }

            return (2 * (double)(counter)) / ((2 + (double)(reference.Length)) + (2 + (double)(text.Length))); //to prevent calculation issues, all 'int' type values have to be casted to 'double' type first

        }

        /// <summary>
        /// returns the trigram-based best match from a list of string options given (respecting a threshold value optionally)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="options"></param>
        /// <param name="threshold"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        private static string? GetBestMatchUsingTrigrams(string text, IEnumerable<string> options, out double dist, double threshold = 0)
        {

            KeyValuePair<string, double> output = default(KeyValuePair<string, double>); //use the default values {null,0}

            foreach (string option in options)
            {

                double similarity = TrigramSimilarityTo(text,option);

                if (similarity > output.Value)
                {
                    output = new KeyValuePair<string, double>(option, similarity);
                }

            }

            dist = output.Value;

            if (output.Value >= threshold)
            {
                return output.Key;
            }

            return null;

        }

        private static string? GetBestMatchUsingTrigrams(string text, IEnumerable<string> options, double threshold = 0)
        {
            double result = 0;
            return GetBestMatchUsingTrigrams(text,options,out result,threshold);
        }

        #endregion

        #region private method(s) supporting levenshtein-based similarity

        /// <summary>
        /// calculates the normalized Levenshtein distance between two strings.
        /// </summary>
        /// <param name="first">The first string.</param>
        /// <param name="second">The second string.</param>
        /// <returns>The normalized Levenshtein distance between the two strings.</returns>
        private static double LevenshteinDistanceNormalizedTo(string first, string second)
        {
            var matrix = new int[first.Length + 1, second.Length + 1];

            for (var i = 0; i <= first.Length; i++)
            {
                matrix[i, 0] = i;
            }

            for (var j = 0; j <= second.Length; j++)
            {
                matrix[0, j] = j;
            }

            for (var i = 1; i <= first.Length; i++)
            {
                for (var j = 1; j <= second.Length; j++)
                {
                    var cost = (first[i - 1] == second[j - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            var totalCost = matrix[first.Length, second.Length];
            var averageLength = (first.Length + second.Length) / 2.0;

            return (totalCost / (double)averageLength);
        }

        /// <summary>
        /// returns the trigram-based best match from a list of string options given (respecting a threshold value optionally)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="options"></param>
        /// <param name="threshold"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        private static string? GetBestMatchUsingLevenshteinDistanceNormalized(string text, IEnumerable<string> options, out double dist, double? threshold = 0)
        {
            KeyValuePair<string, double> output = new KeyValuePair<string, double>("", 10); //use a default with distance of 10 to ensure the matches are updated

            foreach (string option in options)
            {
                var distance = LevenshteinDistanceNormalizedTo(text, option);

                if (distance < output.Value)
                {
                    output = new KeyValuePair<string, double>(option, distance);
                }
            }
            dist = output.Value;

            // Respect the threshold if it is provided
            if (output.Value > threshold)
            {
                return null;
            }

            return output.Key;
        }

        private static string? GetBestMatchUsingLevenshteinDistanceNormalized(string text, IEnumerable<string> options, double? threshold = 0)
        {
            double result = 0;
            return GetBestMatchUsingLevenshteinDistanceNormalized(text, options, out result, threshold);
        }
        #endregion

    }
}
