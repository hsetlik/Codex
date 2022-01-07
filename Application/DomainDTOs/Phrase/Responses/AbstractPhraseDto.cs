using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Phrase.Responses
{
    public class AbstractPhraseDto
    {
        public bool HasPhrase { get; set; }
        public string Value { get; set; }
        public string Language { get; set; }
        public string ReccomendedTranslation { get; set; }
        public PhraseDto Phrase { get; set; }
    }
}