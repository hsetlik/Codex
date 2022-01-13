using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Content.Responses
{
    public class StylesheetFile
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class ContentPageHtml
    {
        public string ContentUrl { get; set; }
        public string Html { get; set; }
        public List<StylesheetFile> Stylesheets { get; set; }
    }
}