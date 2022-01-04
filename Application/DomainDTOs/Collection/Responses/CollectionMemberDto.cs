using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Collection.Responses
{
    public class CollectionMemberDto
    {
        //Nav properties / foreign keys
        public Guid CollectionId { get; set; }

        public Guid ContentId { get; set; }
        public ContentMetadataDto Content { get; set; }
    }
}