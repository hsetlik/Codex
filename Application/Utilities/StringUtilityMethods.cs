using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class StringUtilityMethods
    {
        // returns a version of the string with punctuation removed in all caps
        public static string AsTermValue(string input)
        {
            var match = Regex.Match(input, @"([^\p{P}^\s]+)");
            if (!match.Success)
                return "No valid characters in string";
            return match.Value.ToUpper();
        }   

        public static string GetTrailing(string input)
        {
           var match = Regex.Match(input, @"([^\{P}^\s]+)");
            if (!match.Success)
                return "No valid characters in string";
            if (match.Value.Length < input.Length)
            {
                return input.Substring(match.Value.Length);
            }
            return "";
        }

        public static string StripWikiAnnotations(string input)
        {
            const string expression = @"(&#)([\w;]+)";
            return StrippedOfMatches(input, expression);
        }

        private static string StrippedOfMatches(string input, string pattern)
        {
            return Regex.Replace(input, pattern, "");
        }

    }
}