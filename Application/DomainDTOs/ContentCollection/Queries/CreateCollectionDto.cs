using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.ContentCollection.Queries
{
    public class CreateCollectionDto
    {
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public Guid CreatorProfileId { get; set; }
        public string FirstContentUrl { get; set; }

    }
}