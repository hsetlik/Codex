using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Translator
{

    public class TranslatorResponse
    {
        public TranslatorQuery Query { get; set; }
        public string ResponseValue { get; set; }
    }
}