using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Application.Utilities;

namespace Application.Parsing.ProfileParsers
{
   //Wikipedia
    public class WikipediaContentParser : HtmlContentParser
    {
        public WikipediaContentParser(string url) : base(url)
        {
        }

        public override async Task<int> GetNumParagraphs()
        {
           if (!HasLoadedHtml)
                await LoadHtml();
            return loadedHtml.DocumentNode.Descendants("p").ToList().Count;
        }

        public override async Task<ContentParagraph> GetParagraph(int index)
        {
             if (!HasLoadedHtml)
                await LoadHtml();
            var paragraph = loadedHtml.DocumentNode.Descendants("p").ElementAt(index);
            bool headerFound = false;
            var headerSibling = paragraph.PreviousSibling;
            var headerString = "none";
            while (headerSibling != null && !headerFound)
            {
                if (headerSibling.Name == "h2")
                {
                   var titleSpan = headerSibling.Descendants("span").FirstOrDefault(s => s.HasClass("mw-headline"));
                   if (titleSpan != null)
                   {
                       headerString = titleSpan.InnerText;
                       headerFound = true;
                   }
                }
                else if (headerSibling.HasClass("toc"))
                {
                    headerFound = true;
                }
                headerSibling = headerSibling.PreviousSibling;
            }
            return new ContentParagraph
            {
                ContentUrl = Url,
                Index = index,
                Value = StringUtilityMethods.StripWikiAnnotations(paragraph.InnerText),
                ParagraphHeader = headerString
            };
        }

        public override async Task<ContentMetadataDto> GetMetadata()
        {
            Console.WriteLine("GETTING WIKIPEDIA METADATA...");
            if (!HasLoadedHtml)
                await LoadHtml();
            var name = loadedHtml.DocumentNode.DescendantsAndSelf().SingleOrDefault(u => u.Name == "title").InnerText;
            var lang = loadedHtml.DocumentNode.Descendants("html").FirstOrDefault().GetAttributeValue("lang", "not found");
            return new ContentMetadataDto
            {
                ContentName = name,
                ContentType = "Wikipedia",
                Language = lang,
                VideoUrl = "none",
                AudioUrl = "none",
                ContentUrl = Url
            };
        }
    }

}