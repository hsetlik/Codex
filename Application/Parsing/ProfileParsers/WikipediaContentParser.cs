using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Application.Extensions;
using Application.Parsing.ContentStorage;
using Application.Utilities;

namespace Application.Parsing.ProfileParsers
{
   //Wikipedia

    public class WikipediaContentParser : HtmlContentParser
    {

        private WikiContentStorage storage;
        public WikipediaContentParser(string url) : base(url)
        {
        }

        public override async Task<int> GetNumSections()
        {
           if (!HasLoadedHtml)
                await LoadHtml();
            var parserOutputNode = loadedHtml.DocumentNode.Descendants("div").FirstOrDefault(n => n.HasClass("mw-parser-output"));
            var sectionHeaders = parserOutputNode.Descendants("span").Where(n => n.HasClass("mw-headline")).ToList();
            return sectionHeaders.Count + 1; //add one because we will always have an intro paragraph which does not have a mw-headline header
        }

        public override async Task<ContentSection> GetSection(int index)
        {
            if (!HasLoadedHtml)
                await LoadHtml();
            string headerString = "none";
            string sectionContent = "";
            var parserOutputNode = loadedHtml.DocumentNode.Descendants("div").FirstOrDefault(n => n.HasClass("mw-parser-output"));
            if (index == 0)
            {
                //in this case, just take the intro paragraphs
                var paragraphNode = parserOutputNode.Descendants("p").FirstOrDefault();
                while (paragraphNode.Name == "p")
                {
                    sectionContent += paragraphNode.InnerText;
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
                sectionContent = sectionHeaders[index].ContentUnderHeaderWiki().Value;
            }
            return new ContentSection
            {
                ContentUrl = Url,
                Index = index,
                Value = StringUtilityMethods.StripWikiAnnotations(sectionContent),
                SectionHeader = headerString
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

        public override async Task ParseToContent()
        {
            // get metadata first
            Console.WriteLine("GETTING WIKIPEDIA METADATA...");
            if (!HasLoadedHtml)
                await LoadHtml();
            var name = loadedHtml.DocumentNode.DescendantsAndSelf().SingleOrDefault(u => u.Name == "title").InnerText;
            var lang = loadedHtml.DocumentNode.Descendants("html").FirstOrDefault().GetAttributeValue("lang", "not found");
            storage.Metadata = new ContentMetadataDto
            {
                ContentName = name,
                ContentType = "Wikipedia",
                Language = lang,
                VideoUrl = "none",
                AudioUrl = "none",
                ContentUrl = Url
            };

            //Now to grip the sections
            
        }
    }

}