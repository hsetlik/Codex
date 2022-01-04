using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class Collection
    {
        [Key]
        public Guid CollectionId { get; set; }

        // Nav property
        public UserLanguageProfile UserLanguageProfile { get; set; }
        public Guid LanguageProfileId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Language { get; set; }
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public ICollection<Content> Contents { get; set; } = new List<Content>();
    }
}