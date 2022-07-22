using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content.Queries
{
    public class SaveContentQuery
    {
        public string ContentUrl { get; set; }
        public Guid LanguageProfileId { get; set; }
    }
}