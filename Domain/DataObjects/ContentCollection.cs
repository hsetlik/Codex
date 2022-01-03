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
        public Guid CreatorProfileId { get; set; }
        public string CreatorUsername { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Content> Contents { get; set; } = new List<Content>();
        public string CollectionName { get; set; }
        public string Description { get; set; }
    }
}