using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;

namespace Application.DomainDTOs.Content
{
    public class AbstractTermsFromSection
    {
        public string ContentUrl { get; set; }
        public int Index { get; set; }
        public List<AbstractTermDto> AbstractTerms { get; set; }
    }
}