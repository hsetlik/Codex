using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;

namespace Application.DomainDTOs.Phrase
{
    public class PhraseCreateQuery
    {
        public string Language { get; set; }
        public string Value { get; set; }
        public string FirstTranslation { get; set; }
    }
}