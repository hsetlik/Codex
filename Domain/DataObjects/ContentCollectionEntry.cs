using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class ContentCollectionEntry
    {
        [Key]
        public Guid ContentCollectionEntryId { get; set; }
        public DateTime AddedAt { get; set; }

        public Guid LanguageProfileId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }

        public Guid ContentId { get; set; }
        public Content Content { get; set; }
    }
}