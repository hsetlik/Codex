using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content.Queries
{
    public class SectionAtSecondsQuery
    {
        public string ContentUrl { get; set; }
        public int Seconds { get; set; }
    }
}