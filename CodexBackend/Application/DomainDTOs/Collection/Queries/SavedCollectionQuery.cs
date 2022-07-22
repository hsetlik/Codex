using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Collection.Queries
{
    public class SavedCollectionQuery
    {
        public Guid CollectionId { get; set; }
        public Guid LanguageProfileId { get; set; }

    }
}