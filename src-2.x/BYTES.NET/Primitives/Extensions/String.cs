using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives.Extensions
{
    /// <summary>
    /// a set of 'String' type extensions and helper method(s)
    /// </summary>
    public static class String
    {
        #region public method(s)

#if NETFULL

        /// <summary>
        /// removes all leading or trailing instances of a character from the current string 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        /// <remarks>a polymorph implementation for .NET 4.6/ 4.8</remarks>
        public static string Trim(this string text,char character)
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
        /// returns a zero-based list of all indexes of all instances of a specific character
        /// </summary>
        /// <param name="str"></param>
        /// <param name="character"></param>
        /// <returns>based on an article found at 'https://stackoverflow.com/questions/2641326/finding-all-positions-of-substring-in-a-larger-string-in-c-sharp', ignoring overlapping occurences</returns>
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
        /// return a zero-based list of all indexes of all matches for the expression given
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
        /// checks (case in-sensitive) if a 'string' type array contains a specific value
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
        /// expands variables inside a 'String' type instance given
        /// </summary>
        /// <param name="text"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string Expand(this System.String text, Dictionary<string, string> variables, bool ignoreCase = true)
        {

            string output = text;

            foreach(KeyValuePair<string,string> pair in variables)
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
        public static bool MatchesPattern(this string text, Regex pattern, out Match ?match)
        {
            foreach(Match myMatch in pattern.Matches(text))
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
            return MatchesPattern(text, pattern,out _);
        }

        /// <summary>
        /// calculates the trigam-based similarity to another string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        /// <remarks>see 'https://www.innodox.com/de/blog/meier-ist-nicht-gleich-maier-ist-nicht-gleich-mayer-aehnlichkeitssuche-im-alltagstest/' for more details</remarks>
        public static double SimilarityTo(this System.String text, string reference)
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
        /// calculates the trigam-based similarity to another string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        /// <remarks>>1st overload, accepting a 'string' type array as reference input</remarks>
        public static Dictionary<string, double> SimilarityTo(this System.String text, string[] reference)
        {

            Dictionary<string, double> output = new Dictionary<string, double>();

            foreach (string referenceString in reference)
            {
                output.Add(referenceString, text.SimilarityTo(referenceString));
            }

            return output;

        }

        /// <summary>
        /// returns the best match from a given list of string options (respecting a threshold)
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="options"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static string GetBestMatch(this System.String text, string[] options, double threshold = 0)
        {

            KeyValuePair<string, double> output = default(KeyValuePair<string, double>); //use the default values {null,0}

            foreach (string option in options)
            {

                double similarity = text.SimilarityTo(option);

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
