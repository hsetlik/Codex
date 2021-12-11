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

        private struct WikiSectionHeader
        {
            public HtmlNode Node { get; set; }
            public string Name { get; set; }

            public WikiSectionHeader(HtmlNode _node, string _name)
            {
                this.Node = _node;
                this.Name = _name;
            }
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

            // 1. get the metadata
            storage.Metadata = new ContentMetadataDto
            {
                ContentUrl = this.Url,
                ContentName = contentName,
                ContentType = "Wikipedia",
                Language = lang,
                AudioUrl = "none",
                VideoUrl = "none"
            };
            // don't forget that the list needs to exist before we can add to it
            storage.Sections = new List<ContentSection>();
            // stick all the paragraph nodes into a big list
            var mainBody = topNode.Descendants().FirstOrDefault(n => n.HasClass("mw-parser-output"));
            //TODO: this needs to select the first p node at  the app
            var allParagraphs = mainBody.CssSelect("p").ToList();
            for(int i = 0; i < allParagraphs.Count; ++i)
            {
                Console.Write($"PARAGRAPH #{i}: {allParagraphs[i].InnerText}");
            }
            // grab the TOC node
            string introBody = "";
            var tableOfContents = mainBody.CssSelect("div.toc").FirstOrDefault();
            // make sure we don't start with a nested p element inside something else
            var pNode = allParagraphs.FirstOrDefault(p => p.ParentNode == mainBody);
            while (pNode != tableOfContents && pNode != null)
            {
                if (pNode.Name == "p" && pNode.InnerText.Length > 0)
                {
                    Console.WriteLine($"ADDING INTRO PARAGRAPH: {pNode.InnerText}");
                    introBody += pNode.InnerText;
                }
                pNode = pNode.NextSibling;
            }
            Console.WriteLine("INTRO FINISHED");
            if (introBody.Length > 0)
            {
                storage.Sections.Add(new ContentSection
                {
                    ContentUrl = this.Url,
                    Index = 0,
                    Value = StringUtilityMethods.StripWikiAnnotations(introBody),
                    SectionHeader = "none"
                });
            }
            var sectionHeaders = new List<WikiSectionHeader>();
            // grab all the span elements w/ class "mw-headline"
            var sectionSpans = mainBody.CssSelect("span.mw-headline").ToList();
            foreach(var span in sectionSpans)
            {
                var potentialHeader = span.ParentNode;
                Console.WriteLine($"Potential header has name : {potentialHeader.Name}");
                Console.WriteLine($"Span has text: {span.InnerText}");
                if (potentialHeader.Name == "h2" || potentialHeader.Name == "h3")
                {
                    sectionHeaders.Add(new WikiSectionHeader(potentialHeader, span.InnerText));
                }
            }
            // now loop through all the headers and get their paragraphs
            HtmlNode refNode = mainBody.CssSelect("div.reflist").FirstOrDefault();
            for(int i = 0; i < sectionHeaders.Count; ++i)
            {
                // determine which node will be our indication to break the loop
                var endNode = (i == sectionHeaders.Count - 1) ? refNode : sectionHeaders[i + 1].Node;
                string sectionBody = "";
                var current = sectionHeaders[i].Node.NextSibling;
                // iterate and add each paragraph until we hit the end
                while (current != endNode && current != null)
                {
                    if (current.Name == "p")
                    {
                        sectionBody += StringUtilityMethods.StripWikiAnnotations(current.InnerText);
                    }
                    current = current.NextSibling;
                }
                // create the ContentSection object
                storage.Sections.Add(new ContentSection
                {
                    ContentUrl = this.Url,
                    Index = i + 1, // +1 because the intro paragraph is already at index 0
                    Value = sectionBody,
                    SectionHeader = sectionHeaders[i].Name
                });
            }
            contentsLoaded = true;
        }
    }
}