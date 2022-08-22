using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public class SplitString
        {
            public static Match GetMatchWithExceptions(string input, string pattern)
            {
                Match output = null;
                try
                {
                    output = Regex.Match(input, pattern);
                }
                catch (ArgumentNullException argNull)
                {
                    Console.WriteLine($"Argument null! Input {input} with pattern {pattern} not valid on param name: {argNull.ParamName}");
                    throw argNull;
                }
                catch (ArgumentException argExc)
                {
                    Console.WriteLine($"Argument Exception! Input {input} with pattern {pattern} has bad argument with data: {argExc.ToString()}");
                    throw argExc;
                }
                catch (RegexMatchTimeoutException timeoutExc)
                {
                    Console.WriteLine($"Matching input {input} to pattern {pattern} timed out: {timeoutExc.Message}");
                    throw timeoutExc;
                }
                return output;
            }
            public string Word;
            public string Trailing;
            public string Leading;

            public SplitString(string input)
            {
                Trailing = "none";
                Leading = "none";
                var temp = input;
                var leadingMatch = GetMatchWithExceptions(input, @"^[\p{P}\s]+");
                if (leadingMatch.Success)
                {
                    Leading = leadingMatch.Value;
                    temp = temp.Substring(Leading.Length);
                }
                var trailingMatch = GetMatchWithExceptions(input, @"[\p{P}\s]+$");
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