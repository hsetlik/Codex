using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Extensions;
using Application.Parsing.ContentStorage;
using Application.Utilities;
using HtmlAgilityPack;
using MediatR;
using ScrapySharp;
using ScrapySharp.Extensions;
using ScrapySharp.Network;


namespace Application.Parsing.ProfileScrapers
{
   //Wikipedia

    public class WikipediaContentScraper : AbstractScraper
    {
        private WikiContentStorage storage;
        public WikipediaContentScraper(string url) : base(url)
        {
            storage = new WikiContentStorage();
        }

        public override ContentMetadataDto GetMetadata()
        {
            return storage.Metadata;
        }

        public override int GetNumSections()
        {
            return storage.Sections.Count;
        }

        public override ContentSection GetSection(int index)
        {
            return storage.Sections[index];
        }

        public override async Task PrepareAsync()
        {
            // load the web page
            var sBrower = new ScrapingBrowser();
            var page = await sBrower.NavigateToPageAsync(new Uri(this.Url));
            //grab the HTML
            var topNode = page.Html;
            //sort out the metadata first
            var contentName = topNode.OwnerDocument.DocumentNode.SelectSingleNode("//html/head/title").InnerText;
            var lang = topNode.OwnerDocument.DocumentNode.SelectSingleNode("//html").GetAttributeValue<string>("lang", "not found");

            storage.Metadata = new ContentMetadataDto
            {
                ContentUrl = this.Url,
                ContentName = contentName,
                ContentType = "Wikipedia",
                Language = lang,
                AudioUrl = "none",
                VideoUrl = "none"
            };
            // stick all the paragraph nodes into a big list
            var mainBody = topNode.Descendants().FirstOrDefault(n => n.HasClass("mw-parser-output"));
            var allParagraphs = mainBody.CssSelect("p").ToList();
            //get the first one and start making the intro section
            var firstSection = new ContentSection
            {
                ContentUrl = this.Url,
                Index = 0,
                SectionHeader = "none",
                Value= ""
            };
            var index = 0;
            var current = allParagraphs[index];
            while (
            current != null &&
            index < allParagraphs.Count - 1)
            {
                //TODO: figure out a way to loop through any children here and get rid of the uglies
                var text = StringUtilityMethods.StripWikiAnnotations(current.InnerText);
                //Console.WriteLine($"Current paragraph element: {text}");
                firstSection.Value += text;
                ++index;
                current = allParagraphs[index];
                if (current.NextSibling == null || current.NextSibling.GetAttributeValue("class", "not class") == "toc")
                    current = null;
            }
            storage.Sections = new List<ContentSection>();
            storage.Sections.Add(firstSection);
            //grab all the section headers and sub-headers for now
            var depth = mainBody.Depth;
            var allSectionHeaders = mainBody.Descendants().Where(n => (n.Name == "h2" || n.Name == "h3")).ToList();
            foreach(var header in allSectionHeaders)
            {
                var h = header.CssSelect("span").FirstOrDefault();
                var headerName = (h == null) ? "ERROR" : h.InnerText;
                Console.WriteLine($"Header name is: {headerName}");
                var section = new ContentSection
                {
                    ContentUrl = this.Url,
                    Index = allSectionHeaders.IndexOf(header) - 1,
                    SectionHeader = headerName,
                    Value =""
                };
                var p = header.NextSibling;
                while (p != null && p.Name != "h2" && p.Name != "h3")
                {
                    if (p.Name == "p")
                    {
                        section.Value += StringUtilityMethods.StripWikiAnnotations(p.InnerText);
                    }
                    p = p.NextSibling;
                }
                if (section.Value.Length > 0)
                    storage.Sections.Add(section);
            }
            contentsLoaded = true;
        }
    }

}