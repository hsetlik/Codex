using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content.Queries
{
    public class ContentDifficultyQuery
    {
        public Guid LanguageProfileId { get; set; }
        public Guid ContentId { get; set; }
    }
}