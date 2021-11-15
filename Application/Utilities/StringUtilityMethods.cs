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
    }
}