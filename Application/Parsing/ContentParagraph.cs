using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Parsing
{
    public class ContentParagraph
    {
        public string ContentUrl { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public string ParagraphHeader { get; set; } // this should be "none" if there is no header
    }
}