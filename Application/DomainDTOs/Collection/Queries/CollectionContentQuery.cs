using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Collection.Queries
{
    public class CollectionContentQuery
    {
        public Guid CollectionId { get; set; }
        public string ContentUrl { get; set; }
    }
}