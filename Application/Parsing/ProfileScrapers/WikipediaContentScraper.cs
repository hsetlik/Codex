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

        public override async Task PrepareAsync()
        {
            // load the web page
            var sBrower = new ScrapingBrowser();
            var page = await sBrower.NavigateToPageAsync(new Uri(this.Url));
          
            //grab the HTML
            var root = page.Html;
            //grab the head node
            var head = root.CssSelect("head").FirstOrDefault();
            //get the stylesheets
            var stylesheets = root.Descendants().Where(d => d.Attributes.Any(a => a.Value == "stylesheet")).ToList();
            foreach(var sheet in stylesheets)
            {

                var urlPrefix = Url.Substring(0, Url.IndexOf(@"wiki/") - 1);
                var rel = sheet.GetAttributeValue("href");
                var sheetUrl = urlPrefix + rel;
                Console.WriteLine($"Stylesheet URL is:{sheetUrl}");
                var stylesheetPage = await sBrower.NavigateToPageAsync(new Uri(sheetUrl));
                var pageContent = stylesheetPage.Content;
                Console.WriteLine($"Page Content is: {pageContent.Substring(0, 100)}");
            }
            //get the full node inside the <html> tag
            var htmlNode = root.CssSelect("body").FirstOrDefault();
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
                TextElements = new List<VideoCaptionElement>()
            };
            foreach (var node in bodyNodesOrdered)
            {
                // if we've hit a header, then the last node was the end of the previous section
                node.SetAttributeValue("codex_replacable", "true");
                string inner = node.InnerText.StripWikiAnnotations();
                if (node.Name == "h1" || node.Name == "h2") 
                {
                    storage.Sections.Add(currentSection);
                    currentSection = new ContentSection
                    {
                        ContentUrl = Url,
                        Index = storage.Sections.Count,
                        SectionHeader = inner,
                        TextElements = new List<VideoCaptionElement>()
                    };
                }
                currentSection.TextElements.Add(new VideoCaptionElement
                {
                    ElementText = inner,
                    Tag = node.Name,
                    ContentUrl = Url,
                    Index = currentSection.TextElements.Count
                });
            }          
            storage.Metadata.NumSections = storage.Sections.Count;
            storage.RawPageHtml = mainBody.OuterHtml;
            contentsLoaded = true;
        }

        public override List<ContentSection> GetAllSections()
        {
            return storage.Sections;
        }

        public override string GetHtmlString()
        {
            return storage.RawPageHtml;
        }
    }
}