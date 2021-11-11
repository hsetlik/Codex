using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain.DataObjects;
using MediatR;

namespace Application.Extensions
{
    public static class UserTermExtensions
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

        
        
    }
}