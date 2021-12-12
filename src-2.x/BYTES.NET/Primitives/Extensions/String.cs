using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives.Extensions
{
    public static class String
    {

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

    }
}
