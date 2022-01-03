using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class ContentCollection
    {
        [Key]
        public Guid ContentCollectionId { get; set; }
        public UserLanguageProfile UserLanguageProfile { get; set; }
        public Guid LanguageProfileId { get; set; }
        public string CreatorUsername { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<ContentCollectionEntry> Entries { get; set; } = new List<ContentCollectionEntry>();
        public string CollectionName { get; set; }
        public string Description { get; set; }
    }
}