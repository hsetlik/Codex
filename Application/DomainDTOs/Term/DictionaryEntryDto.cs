using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Term
{
    public class DictionaryEntryDto
    {
        public string TermValue { get; set; }
        public string TermLanguage { get; set; }
        public string TranslationValue { get; set; }
    }
}