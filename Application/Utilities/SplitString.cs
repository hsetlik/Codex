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
                Word = Regex.Match(input, @"([^\p{P}^\s]+)").Value;
                Leading = "none";
                Trailing = "none";
                if(Word.Length < 1)
                    return;
                if (input.Length > Word.Length)
                {
                    int leadLength = 0;
                    if(input[0] != Word[0])
                    {
                        leadLength = input.IndexOf(Word[0]);
                        Leading = input.Substring(0, leadLength);
                    }
                    if (input.Length - leadLength > Word.Length)
                    {
                        Trailing = input.Substring(leadLength + Word.Length);
                        //Word = Word.Substring(0, Word.Length - Trailing.Length);
                    }
                }
                               
            }
        }
}