using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content
{
    public class ContentSectionMetadata
    {
        public string ContentUrl { get; set; }
        public string SectionHeader { get; set; }
        public int Index { get; set; }
        public int NumElements { get; set; }
    }
}