using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content.Queries
{
    public class SectionAtMsQuery
    {
        public string ContentUrl { get; set; }
        public int Ms { get; set; }
    }
}