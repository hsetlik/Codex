using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Translator
{
    public class TranslatorQuery
    {
        public string ResponseLanguage { get; set; }
        public string QueryLanguage { get; set; }
        public string QueryValue { get; set; }
    }
}