using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;

namespace Application.DomainDTOs.Content
{
    public class ElementAbstractTerms
    {
        public string Tag { get; set; }
        public List<AbstractTermDto> AbstractTerms { get; set; }
    }
    public class SectionAbstractTerms
    {
        public string ContentUrl { get; set; }
        public int Index { get; set; }
        public string SectionHeader { get; set; }
        // for functionality with HTML tags
        public List<ElementAbstractTerms> ElementGroups { get; set; }
    }
}