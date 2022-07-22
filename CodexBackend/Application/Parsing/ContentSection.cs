using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Application.Parsing
{
    public class TextElement 
    {
        public string Tag { get; set; }
        public string ElementText { get; set; }
        public string ContentUrl { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public TextElement()
        {

        }
        public TextElement(HtmlNode node, string contentUrl)
        {
            this.ContentUrl = contentUrl;
            this.Tag = node.NodeType.ToString();
            this.ElementText = node.InnerText;
            this.Attributes = new Dictionary<string, string>();
            if (node.HasAttributes)
            {
                foreach(var att in node.Attributes)
                {
                    this.Attributes[att.Name] = att.Value;
                }
            }
        }
    }
    public class VideoCaptionElement
    {
        public VideoCaptionElement()
        {
        }

        public string CaptionText { get; set; }

        public int StartMs { get; set; }
        public int EndMs { get; set; }
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
                full += e.ElementText + ' ';
            }
            return full;
        }}
    }
}