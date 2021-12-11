using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public class SplitString
        {
            public string Word;
            public string Trailing;
            public string Leading;

            public SplitString(string input)
            {
                Trailing = "none";
                Leading = "none";
                var temp = input;
                Word = Regex.Match(input, @"([^\p{P}^\s]+)").Value;
               
                var leadingMatch = Regex.Match(input, @"^[\p{P}\s]+");
                if (leadingMatch.Success)
                {
                    Leading = leadingMatch.Value;
                    temp = temp.Substring(Leading.Length);
                }
                var trailingMatch = Regex.Match(input, @"[\p{P}\s]+$");
                if (trailingMatch.Success)
                {
                    Trailing = trailingMatch.Value;
                    var length = temp.Length - Trailing.Length;
                    if (length >= 1)
                        temp = temp.Substring(0, length);
                }
                Word = temp;           
            }
        }
}