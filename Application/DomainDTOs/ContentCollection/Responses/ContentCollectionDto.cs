using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.ContentCollection.Responses
{
    public class ContentCollectionDto
    {
        public Guid ContentCollectionId { get; set; }
        public Guid LanguageProfileId { get; set; }
        public string CreatorUsername { get; set; }
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ContentMetadataDto> CollectionContents { get; set; }

    }
}