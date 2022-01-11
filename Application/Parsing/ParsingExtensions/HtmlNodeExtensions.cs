using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Application.Parsing.ParsingExtensions
{
    public static class HtmlNodeExtensions
    {
        public static VideoCaptionElement AsTextElement(this HtmlNode node)
        {
            return new VideoCaptionElement
            {
                ElementText = node.InnerText,
                Tag = node.Name
            };
        }
    }
}