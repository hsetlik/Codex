using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.UserLanguageProfile
{
    public class LanguageProfileDto
    {
        public Guid LanguageProfileId { get; set; }
        public string Language { get; set; }
        public int KnownWords { get; set; }
        public string UserLanguage { get; set; }
    }
}