using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Parsing
{
    public class TextElement 
    {
        public string Tag { get; set; }
        public string ElementText { get; set; }
        public string ContentUrl { get; set; }
    }
    public class VideoCaptionElement : TextElement
    {
        public int Index { get; set; }
        public bool HasTimeSpan { get {return Tag == "caption";}}
        public int StartMs { get; set; }
        public int EndMs { get; set; }
    }
    //note: this can be subclassed to do more complicated stuff as needed
    public class ContentSection
    {
        public string ContentUrl { get; set; }
        public int Index { get; set; }
        public string SectionHeader { get; set; }
        public List<VideoCaptionElement> TextElements { get; set; } = new List<VideoCaptionElement>();
        public string Body {get 
        {
            string full = "";
            foreach(var e in TextElements)
            {
                full += e.ElementText + ' ';
            }
            return full;
        }}
    }
}