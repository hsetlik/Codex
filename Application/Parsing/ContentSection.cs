using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Parsing
{
    public class TextElement
    {
        public string Tag { get; set; }
        public string Value { get; set; }
        public string ContentUrl { get; set; }
        public int Index { get; set; }
    }
    //note: this can be subclassed to do more complicated stuff as needed
    public class ContentSection
    {
        public string ContentUrl { get; set; }
        public int Index { get; set; }
        public string SectionHeader { get; set; }
        public List<TextElement> TextElements { get; set; } = new List<TextElement>();
        public string Body {get 
        {
            string full = "";
            foreach(var e in TextElements)
            {
                full += e.Value + ' ';
            }
            return full;
        }}
       
       

    }
}