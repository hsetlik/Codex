using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.UserTerm
{
    public class TranslationDto
    {
        public string TermValue { get; set; } //this is just = UserTerm.NormalizedTermValue
        public string TermLanguage { get; set; }
        public string UserValue { get; set; } // the value of the translation
        public string UserLanguage { get; set; } // the user's native language
       
    }
}