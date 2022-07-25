using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class ContentDifficulty
    {
        // Navigation properties
        public Guid LanguageProfileId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }
        public Guid ContentId { get; set; }
        public Content Content { get; set; }
        // Entity data
        public DateTime UpdatedAt { get; set; }
        public int TotalWords { get; set; }
        public int KnownWords { get; set; }
    }
}