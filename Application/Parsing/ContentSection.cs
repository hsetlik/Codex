using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Parsing
{
    //note: this can be subclassed to do more complicated stuff as needed
    public class ContentSection
    {
        public string ContentUrl { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public string SectionHeader { get; set; } 
        public ContentSection(string _url="none", int _index=0, string _value="", string _sectionHeader="none")
        {
            this.ContentUrl = _url;
            this.Index = _index;
            this.Value = _value;
            this.SectionHeader = _sectionHeader;
        }
    }
}