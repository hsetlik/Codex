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
        
        public static string[] TextElementParentTypes =
        {
            "p",
            "h1",
            "h2",
            "h3",
            "span",
            "li"
        };

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
                Console.WriteLine($"Stylesheet URL is: {sheetUrl}");
            }
            //get the full node inside the <html> tag
            var htmlNode = root.CssSelect("body").FirstOrDefault();
            var contentName = root.OwnerDocument.DocumentNode.SelectSingleNode("//html/head/title").InnerText;
            var lang = root.OwnerDocument.DocumentNode.SelectSingleNode("//html").GetAttributeValue<string>("lang", "not found");
            /* 

            Bad URL:  https://ru.wikipedia.org/w/load.php?lang=ru&amp;modules=ext.cite.styles%7Cext.flaggedRevs.basic%2Cicons%7Cext.kartographer.style%7Cext.uls.interlanguage%7Cext.visualEditor.desktopArticleTarget.noscript%7Cext.wikimediaBadges%7Cmediawiki.widgets.styles%7Coojs-ui-core.icons%2Cstyles%7Coojs-ui.styles.indicators%7Cskins.vector.styles.legacy%7Cwikibase.client.init&amp;only=styles&amp;skin=vector
            Good URL: https://ru.wikipedia.org/w/load.php?lang=ru&modules=ext.cite.styles%7Cext.flaggedRevs.basic%2Cicons%7Cext.kartographer.style%7Cext.uls.interlanguage%7Cext.visualEditor.desktopArticleTarget.noscript%7Cext.wikimediaBadges%7Cmediawiki.widgets.styles%7Coojs-ui-core.icons%2Cstyles%7Coojs-ui.styles.indicators%7Cskins.vector.styles.legacy%7Cwikibase.client.init&only=styles&skin=vector    
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
            foreach(string name in TextElementParentTypes)
            {
                var nodes = htmlNode.CssSelect(name).ToList();
                foreach(var node in nodes)
                {
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