using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;

namespace Application.Parsing.ContentStorage
{
    public class BaseContentStorage
    {
        public ContentMetadataDto Metadata { get; set; }
        public List<ContentSection> Sections { get; set; }
        public string RawPageHtml { get; set; }

        
    }
}