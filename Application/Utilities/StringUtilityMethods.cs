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
        public static string AsTermValue(this string input)
        {
            var split = new SplitString(input);
            return split.Word.ToUpper();            
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
            var output = StrippedOfMatches(input, expression);
            var words = output.Split(' ');
            return StrippedOfMatches(input, expression);
        }
        
        private static string StrippedOfMatches(string input, string pattern)
        {
            return Regex.Replace(input, pattern, "");
        }

       

    }
     
}