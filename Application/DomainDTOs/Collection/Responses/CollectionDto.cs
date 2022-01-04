using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Collection.Responses
{
    public class CollectionDto
    {
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
        public List<ContentMetadataDto> Contents { get; set; }
    }
}