using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.Content.Responses;
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
        private List<TextElement> textElements = new List<TextElement>();
        public static class ContentElementTags
        {
            public static string[] Tags =
            {
                "p",
                "h1",
                "h2",
                "h3",
                "span",
                "li",
                "td",
                "th",
                "small"
            };


        }
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
            return new ContentSection
            {
                ContentUrl = storage.Metadata.ContentUrl,
                Index = index,
                SectionHeader = storage.Metadata.ContentUrl,
                TextElements = textElements
            };
        }
        public override async Task PrepareAsync()
        {
            // load the web page
            var sBrower = new ScrapingBrowser();
            var page = await sBrower.NavigateToPageAsync(new Uri(this.Url));
            //grab the HTML
            var root = page.Html;
            //grab the head node
            // get the style node?
            var styleNodes = root.Descendants("style").ToList();
            var head = root.CssSelect("head").FirstOrDefault();
            //get the stylesheets
            storage.StylesheetUrls = new List<string>();
            var stylesheets = root.Descendants().Where(d => d.Attributes.Any(a => a.Value == "stylesheet")).ToList();
            foreach(var sheet in stylesheets)
            {
                var urlPrefix = Url.Substring(0, Url.IndexOf(@"wiki/") - 1);
                var rel = sheet.GetAttributeValue("href");
                rel = Regex.Replace(rel, @"amp;", "");
                var sheetUrl = urlPrefix + rel;
                storage.StylesheetUrls.Add(sheetUrl);
            }
            //get the full node inside the <html> tag
            var htmlNode = root.CssSelect("body").FirstOrDefault();
            var contentName = root.OwnerDocument.DocumentNode.SelectSingleNode("//html/head/title").InnerText;
            var lang = root.OwnerDocument.DocumentNode.SelectSingleNode("//html").GetAttributeValue<string>("lang", "not found");
            /* 

            */
            // 1. get the metadata
            storage.Metadata = new ContentMetadataDto
            {
                ContentUrl = this.Url,
                ContentName = contentName,
                ContentType = "Wikipedia",
                Language = lang,
                AudioUrl = "none",
                VideoUrl = "none",
                NumSections = 1
            };
            foreach(string name in ContentElementTags.Tags)
            {
                var nodes = htmlNode.CssSelect(name).ToList();
                foreach(var node in nodes)
                {
                    var bodyString = node.InnerText.StripWikiAnnotations().WithoutSquareBrackets();
                    var element = new TextElement
                    {
                        Tag = node.Name,
                        ElementText = bodyString,
                        ContentUrl = this.Url
                    };
                    this.textElements.Add(element);
                    node.SetAttributeValue("codex_replacable", "true");
                }
            }
            var wikiBodyNode = root.CssSelect("div.mw-body").FirstOrDefault();
            storage.RawPageHtml = wikiBodyNode.InnerHtml;
            contentsLoaded = true;
        }

        public override List<ContentSection> GetAllSections()
        {
            return storage.Sections;
        }

        public override ContentPageHtml GetPageHtml()
        {
            return storage.ContentPageHtml;
        }
    }
}