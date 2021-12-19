using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;

namespace Application.DomainDTOs.Phrase
{
    public class PhraseCreateDto
    {
        public List<AbstractTermDto> AbstractTerms { get; set; }
        public string FirstTranslation { get; set; }
    }
}