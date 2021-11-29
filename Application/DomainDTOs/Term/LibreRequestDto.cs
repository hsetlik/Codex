using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Term
{
    public class LibreRequestDto
    {
        public string Q { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
    }

    public class LibreResponseDto
    {
        public string TranslatedText { get; set; }
    }
}