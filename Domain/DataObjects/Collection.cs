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

        // note: this isn't a nav property per se, just a reference to the creator
        public Guid CreatorLanguageProfileId { get; set; }
        public string CreatorUserName { get; set; }
        //Columns
        public bool IsPrivate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Language { get; set; }
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public ICollection<CollectionContent> CollectionMembers { get; set; }
        public ICollection<SavedCollection> SavedCollections { get; set; }
    }
}