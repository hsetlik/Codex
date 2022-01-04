using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs.UserLanguageProfile;

namespace Application.DomainDTOs.ContentCollection.Responses
{
    public class ContentCollectionEntryDto
    {
        public Guid ContentCollectionEntryId { get; set; }
        public Guid LanguageProfileId { get; set; }
        public Guid ContentId { get; set; }
        public DateTime AddedAt { get; set; }

        public LanguageProfileDto LanguageProfileDto { get; set; }
        public ContentMetadataDto ContentMetadataDto { get; set; }

    }
}