using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class Content
    {
        [Key]
        public Guid ContentId { get; set; }
        public string ContentUrl { get; set; }
        public string VideoUrl { get; set; }
        public string AudioUrl { get; set; }
        public string ContentType { get; set; }
        public string ContentName { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ContentTag> ContentTags { get; set; }
        public int NumSections { get; set; }
        public ICollection<CollectionContent> CollectionContents { get; set; }
    }
}