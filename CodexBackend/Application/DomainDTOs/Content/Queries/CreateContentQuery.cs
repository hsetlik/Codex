using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content.Queries
{
    public class CreateContentQuery
    {
        public string ContentUrl { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}