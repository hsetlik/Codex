using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.ContentCollection.Queries
{
    public class AddToCollectionQuery
    {
        public Guid ContentCollectionId { get; set; }
        public string ContentUrl { get; set; }
    }
}