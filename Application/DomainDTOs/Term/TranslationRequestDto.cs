using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Term
{
    public class TranslationRequestDto
    {
        public string TermValue { get; set; }
        public string Language { get; set; }
        public string UserLanguage { get; set; }
    }
}