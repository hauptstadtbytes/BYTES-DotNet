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
        /// calculates the trigam-based similarity compared to a string given
        /// </summary>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        /// <remarks>see 'https://www.innodox.com/de/blog/meier-ist-nicht-gleich-maier-ist-nicht-gleich-mayer-aehnlichkeitssuche-im-alltagstest/' for more details</remarks>
        public static double TrigramSimilarityTo(this System.String text, string reference)
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
        /// calculates the trigam-based similarity compared to a string given, supporting a string array for reference
        /// </summary>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static Dictionary<string, double> TrigramSimilarityTo(this System.String text, string[] reference)
        {

            Dictionary<string, double> output = new Dictionary<string, double>();

            foreach (string referenceString in reference)
            {
                output.Add(referenceString, text.TrigramSimilarityTo(referenceString));
            }

            return output;

        }

        /// <summary>
        /// returns the best match from a given list of string options (optionally respecting a threshold value)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="options"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static string GetBestMatchUsingTrigrams(this System.String text, string[] options, double threshold = 0)
        {

            KeyValuePair<string, double> output = default(KeyValuePair<string, double>); //use the default values {null,0}

            foreach (string option in options)
            {

                double similarity = text.TrigramSimilarityTo(option);

                if (similarity > output.Value)
                {
                    output = new KeyValuePair<string, double>(option, similarity);
                }

            }

            if (output.Value >= threshold)
            {
                return output.Key;
            }

            return System.String.Empty;

        }

        #region Levenshtein Distance

        /// <summary>
        /// Calculates the normalized Levenshtein distance between two strings.
        /// </summary>
        /// <param name="first">The first string.</param>
        /// <param name="second">The second string.</param>
        /// <returns>The normalized Levenshtein distance between the two strings.</returns>
        public static double LevenshteinDistanceNormalized(this string first, string second)
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
            var totalLength = first.Length + second.Length;

            return (totalCost / (double)totalLength);
        }

        /// <summary>
        /// Finds the minimum normalized Levenshtein distance among a collection of strings to a target string.
        /// </summary>
        /// <param name="target">The target string.</param>
        /// <param name="candidates">A collection of candidate strings.</param>
        /// <returns>A dictionary with each candidate string and its corresponding normalized Levenshtein distance to the target string.</returns>
        public static Dictionary<string, double> MinimumLevenshteinDistanceNormalized(this string target, IEnumerable<string> candidates)
        {
            var distances = new Dictionary<string, double>();

            foreach (var candidate in candidates)
            {
                var distance = target.LevenshteinDistanceNormalized(candidate);
                distances[candidate] = distance;
            }

            return distances;
        }

        /// <summary>
        /// Determines whether two strings are similar within a specified threshold.
        /// </summary>
        /// <param name="first">The first string.</param>
        /// <param name="second">The second string.</param>
        /// <param name="threshold">The maximum allowed normalized Levenshtein distance for the strings to be considered similar.</param>
        /// <returns><c>true</c> if the strings are similar within the threshold; otherwise, <c>false</c>.</returns>
        public static bool LevenstheinIsSimilarWithinThreshold(this string first, string second, double threshold)
        {
            var distance = first.LevenshteinDistanceNormalized(second);
            return distance < threshold;
        }

        /// <summary>
        /// Determines the best match between the array of strings to the target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string GetBestMatchUsingLevenshtein(this string target, string[] options)
        {
            // Calculate the normalized Levenshtein distance for each option against the target
            var distances = target.MinimumLevenshteinDistanceNormalized(options);

            // Find the option with the lowest distance
            var bestMatch = distances.OrderBy(pair => pair.Value).First();

            // Return the best match
            return bestMatch.Key ?? string.Empty;
        }

        #endregion

        /// <summary>
        /// Method to select which algorythm to use
        /// </summary>
        /// <param name="algorythm"></param>
        /// <param name="target"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string GetBestMatch(string algorythm, string target, string[] options)
        {
            if (algorythm == "Trigram")
            {
                return GetBestMatchUsingTrigrams(target, options);
            }
            if (algorythm == "Levensthein")
            {
                return GetBestMatchUsingLevenshtein(target, options);
            }
            else
            {
                return "no algorithm found";
            }
        }

        /// <summary>
        /// Method to select what algorythm to use
        /// </summary>
        /// <param name="algorythm"></param>
        /// <param name="target"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static double GetSimilarity(string algorythm, string target, string option)
        {
            if (algorythm == "Trigram")
            {
                return TrigramSimilarityTo(target, option);
            }
            if (algorythm == "Levensthein")
            {
                return LevenshteinDistanceNormalized(target, option);
            }
            else
            {
                return -1;
            }
        }

        #endregion

        #region private method(s)

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

        #endregion
    }
}
