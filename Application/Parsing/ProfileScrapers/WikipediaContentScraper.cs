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
            Console.WriteLine($"Encoding is: {page.Html.OwnerDocument.Encoding.EncodingName}");

            //grab the HTML
            var root = page.Html;
            // TODO 
            var contentName = root.OwnerDocument.DocumentNode.SelectSingleNode("//html/head/title").InnerText;
            var lang = root.OwnerDocument.DocumentNode.SelectSingleNode("//html").GetAttributeValue<string>("lang", "not found");

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
            // 2. grab the main body node
            var mainBody = root.Descendants().FirstOrDefault(n => n.HasClass("mw-parser-output"));
            // 3. Collect all the relevant nodes and ensure they're in DOM order
            var bodyNodes = new List<HtmlNode>();
            bodyNodes.AddRange(mainBody.CssSelect("p"));
            bodyNodes.AddRange(mainBody.CssSelect("h2").Where(n => n.CssSelect("span.mw-headline").Any()));
            bodyNodes.AddRange(mainBody.CssSelect("h3").Where(n => n.CssSelect("span.mw-headline").Any()));
            var bodyNodesOrdered = bodyNodes.OrderBy(n => n.Line).ToList();
            
            //4. create the intro section
            storage.Sections = new List<ContentSection>();
            var currentSection = new ContentSection
            {
                ContentUrl = Url,
                Index = storage.Sections.Count,
                SectionHeader = contentName,
                TextElements = new List<TextElement>()
            };
            foreach (var node in bodyNodesOrdered)
            {
                // if we've hit a header, then the last node was the end of the previous section
                string inner = StringUtilityMethods.StripWikiAnnotations(node.InnerText);
                if (node.Name == "h1" || node.Name == "h2") 
                {
                    inner = StringUtilityMethods.WithoutSquareBrackets(inner);
                    storage.Sections.Add(currentSection);
                    currentSection = new ContentSection
                    {
                        ContentUrl = Url,
                        Index = storage.Sections.Count,
                        SectionHeader = node.InnerText,
                        TextElements = new List<TextElement>()
                    };
                }
                Console.WriteLine($"Node has name {node.Name} and inner text {inner}");
                currentSection.TextElements.Add(new TextElement
                {
                    Value = inner,
                    Tag = node.Name
                });
            }          
            storage.Metadata.NumSections = storage.Sections.Count;
            contentsLoaded = true;
        }

        public override List<ContentSection> GetAllSections()
        {
            return storage.Sections;
        }
    }
}