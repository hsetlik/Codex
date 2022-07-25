using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Collection.Queries
{
    public class CollectionsForLanguageQuery
    {
        public string Language { get; set; }
        public bool EnforceVisibility { get; set; }
    }
}