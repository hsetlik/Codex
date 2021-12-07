using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Application.Extensions;
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
            var parserOutputNode = loadedHtml.DocumentNode.Descendants("div").FirstOrDefault(n => n.HasClass("mw-parser-output"));
            var sectionHeaders = parserOutputNode.Descendants("span").Where(n => n.HasClass("mw-headline")).ToList();
            return sectionHeaders.Count + 1; //add one because we will always have an intro paragraph which does not have a mw-headline header
        }

        public override async Task<ContentParagraph> GetParagraph(int index)
        {
            if (!HasLoadedHtml)
                await LoadHtml();
            string headerString = "none";
            string paragraphContent = "";
            var parserOutputNode = loadedHtml.DocumentNode.Descendants("div").FirstOrDefault(n => n.HasClass("mw-parser-output"));
            if (index == 0)
            {
                //in this case, just take the intro paragraphs
                var paragraphNode = parserOutputNode.Descendants("p").FirstOrDefault();
                while (paragraphNode.Name == "p")
                {
                    paragraphContent += paragraphNode.InnerText;
                    paragraphNode = paragraphNode.NextSibling;
                }
            }
            else
            {
                var sectionHeaders = parserOutputNode.Descendants("span").Where(n => n.HasClass("mw-headline")).ToList();
                for(int i = 0; i < sectionHeaders.Count; ++i)
                {
                    Console.WriteLine($"Section {i} has name: {sectionHeaders[i].InnerText}");
                }
                paragraphContent = sectionHeaders[index].ContentUnderHeaderWiki().Value;
            }
            return new ContentParagraph
            {
                ContentUrl = Url,
                Index = index,
                Value = StringUtilityMethods.StripWikiAnnotations(paragraphContent),
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