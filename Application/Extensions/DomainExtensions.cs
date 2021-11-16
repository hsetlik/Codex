using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Domain.DataObjects;

namespace Application.Extensions
{
    public static class DomainExtensions
    {
       public static void UpdateTranslations(this UserTerm userTerm, List<string> values)
        {
            foreach (var t in values)
            {
                bool exists = false;
                foreach (var translation in userTerm.Translations)
                {
                   if (t == translation.Value)
                   {
                       exists = true;
                       break;
                   }
                }
                if (!exists)
                {
                    var translation = new Translation
                    {
                        Value = t,
                        UserTerm = userTerm
                    };
                    userTerm.Translations.Add(translation);
                }
            }
        }
        
        public static List<Translation> GetAsTranslations(this UserTerm userTerm, List<string> values)
        {
            var output = new List<Translation>();
            foreach(var value in values)
            {
                var translation = new Translation
                {
                    Value = value,
                    UserTerm = userTerm
                };
                output.Add(translation);
            }
            return output;
        }

        // works something like this: https://en.wikipedia.org/wiki/SuperMemo
        public static UserTerm AnsweredWith(this UserTerm input, int answer)
        {
            input.Rating = answer;
            if (answer > 2)
            {
                if (input.TimesSeen == 0)
                    input.SrsIntervalDays = 1.0f;
                else if(input.TimesSeen == 1)
                    input.SrsIntervalDays = 3.0f; //this was six in the original SM-2 algorithm
                else
                    input.SrsIntervalDays = input.SrsIntervalDays * input.EaseFactor;
                input.TimesSeen += 1;
            }
            else
            {
                input.TimesSeen = 0;
                input.SrsIntervalDays = 0.125f; //this is like two hours? maybe should be shorter
            }
            //update ease- coefficients can be tweaked
            input.EaseFactor = input.EaseFactor + (0.1f - (5 - answer) * (0.08f + (5 - answer) * 0.02f));
            //new due date
            var nextDueDate = DateTime.Now.AddDays((double)input.SrsIntervalDays);
            input.DateTimeDue = nextDueDate.ToString();
            return input;
        }
    }
}