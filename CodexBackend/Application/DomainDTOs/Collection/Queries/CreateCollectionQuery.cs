using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Collection.Queries
{
    public class CreateCollectionQuery
    {
        public string CreatorUserName { get; set; }
        //Columns
        public bool IsPrivate { get; set; }
        public string Language { get; set; }
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public string FirstContentUrl { get; set; }
    }
}