using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Application.DomainDTOs.Content.Responses;

namespace Application.Parsing.ContentStorage
{
    public class BaseContentStorage
    {
        public ContentMetadataDto Metadata { get; set; }
        public List<ContentSection> Sections { get; set; }
        public string RawPageHtml { get; set; }
        public List<string> StylesheetUrls { get; set; }

        public ContentPageHtml ContentPageHtml { get 
        {
            return new ContentPageHtml
            {
                ContentUrl = Metadata.ContentUrl,
                Html = RawPageHtml,
                StylesheetUrls = StylesheetUrls
            };
        }}

        
    }
}