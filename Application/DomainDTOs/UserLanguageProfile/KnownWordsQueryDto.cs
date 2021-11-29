using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.UserLanguageProfile
{
    public class KnownWordsQueryDto
    {
        public Guid ContentId { get; set; }
        public string Username { get; set; }
    }
}