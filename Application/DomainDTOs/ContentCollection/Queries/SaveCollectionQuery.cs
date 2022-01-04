using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.ContentCollection.Queries
{
    public class SaveCollectionQuery
    {
        public Guid ContentCollectionId { get; set; }
        public Guid LanguageProfileId { get; set; }
    }
}