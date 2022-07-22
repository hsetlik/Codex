using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content.Responses
{
    public class ContentDifficultyDto
    {
        public Guid LanguageProfileId { get; set; }
        public Guid ContentId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TotalWords { get; set; }
        public int KnownWords { get; set; }
    }
}