using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content
{
    public class ElementTermsQuery
    {
        public string ContentUrl { get; set; }
        public int SectionIndex { get; set; }
        public int ElementIndex { get; set; }
    }
}