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
            return split.Word.Normalize().ToUpper();            
        }   

        public static string EnsureNormalized(this string input)
        {
            if (input.IsNormalized())
                return input;
            return input.Normalize();
        }

        public static string WithoutNewlines(this string input, bool replaceWithAdditionalSpace=false)
        {
           if (replaceWithAdditionalSpace)
           {
               return input.Replace('\n', ' ');
           }
           else
           {
               return StrippedOfMatches(input, "\n");
           }

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

        public static string StripWikiAnnotations(this string input)
        {
            const string expression = @"(&#)([\w;]+)";

            return StrippedOfMatches(input, expression).WithoutSquareBrackets();
        }
        
        public static string WithoutSquareBrackets(this string input)
        {
            const string expression = @"\[([\s\S])+\]";
            return StrippedOfMatches(input, expression);
        }
        private static string StrippedOfMatches(string input, string pattern)
        {
            return Regex.Replace(input, pattern, "");
        }
        

        public static string AsPhraseValue(this string value)
        {
            string output = "";
            var words = value.Split(null).ToList();
            foreach(string word in words)
            {
                output += word.AsTermValue();
                if (word != words.Last())
                    output += ' ';
            }
            return output;
        }

        

       

    }
     
}