using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.Content.Responses;
using Application.Parsing.ContentStorage;
using Application.Parsing.ParsingExtensions;
using CssScraper.Style;
using HtmlAgilityPack;
using MediatR;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using static Application.Parsing.ProfileScrapers.WikipediaContentScraper;

namespace Application.Parsing.ProfileScrapers
{
    // NewsArticle
    
    public class NewsArticleContentScraper : AbstractScraper
    {
        private NewsArticleContentStorage storage;

        public NewsArticleContentScraper(string url) : base(url)
        {
            storage = new NewsArticleContentStorage();
        }
        public override List<ContentSection> GetAllSections()
        {
            return storage.Sections;
        }
        

        public override ContentMetadataDto GetMetadata()
        {
            return storage.Metadata;
        }

        public override int GetNumSections()
        {
            return storage.Sections.Count;
        }

        public override ContentPageHtml GetPageHtml()
        {
            return storage.ContentPageHtml;
        }

        public override string GetPageText()
        {
            return storage.ArticleBodyText;
        }

        public override ContentSection GetSection(int index)
        {
            return storage.Sections[index];
        }

        public override async Task PrepareAsync()
        {
            //  get the HTML
            var downloader = new HttpDownloader(this.Url, null, null);
            var pageText = await Task.Run(() => downloader.GetPage()); 
            Console.WriteLine($"Content URL is : {this.Url}");
            var document = new HtmlDocument();
            document.LoadHtml(pageText);
            var root = document.DocumentNode;
            storage.StylesheetUrls = new List<string>();
            var stylesheets = root.Descendants().Where(d => d.Attributes.Any(a => a.Value == "stylesheet")).ToList();
            var urlRoot = new Uri(Url).DnsSafeHost;
            var rootLong = urlRoot.Substring(0, 8);
            var rootShort = urlRoot.Substring(0, 7);
            Console.WriteLine($"Root Begins with: {rootLong}");
            if (!(rootShort == @"http://" || rootLong == @"https://"))
            {
                string shorter = Url.Substring(0, 7);
                string longer = Url.Substring(0, 8);
                string prefix = (shorter == @"http://") ? shorter : longer;
                urlRoot = prefix + urlRoot;
            }
            foreach(var sheet in stylesheets)
            {
                var rel = sheet.GetAttributeValue("href", "no href");
                rel = Regex.Replace(rel, @"amp;", "");
                Console.WriteLine($"\n\nStylesheet URL: {urlRoot+ rel}");
                storage.StylesheetUrls.Add(urlRoot + rel);
            }
            var headlineNode = root.CssSelect("h1").FirstOrDefault();
            string headline = (headlineNode == null) ? "headline not found" : headlineNode.InnerText;
            var htmlNode = root.CssSelect("html").FirstOrDefault();
            var bodyNode = root.CssSelect("body").FirstOrDefault();
            // create a list of nodes, just grab anything inside relevant HTML tags
            var lang = root.GetAttributeValue("lang", "language not found");
            if (lang == "language not found")
            {

                if (htmlNode != null)
                {
                    lang = htmlNode.GetAttributeValue("lang", "language not found");
                }
            }
            var uNodes = new List<HtmlNode>();
            foreach(var tag in ContentElementTags.Tags)
            {
                uNodes.AddRange(root.CssSelect(tag));
            }
            foreach(var n in uNodes)
            {
                n.SetAttributeValue("codex_replacable", "true");
                storage.ArticleBodyText += n.InnerText;
            }
            storage.Metadata = new ContentMetadataDto
            {
                ContentUrl = this.Url,
                ContentName = headline,
                ContentType = "Article",
                Language = lang,
                Bookmark = 0,
                VideoId = "none",
                AudioUrl = "none",
                NumSections = 1
            };
            storage.RawPageHtml = bodyNode.InnerHtml;
            contentsLoaded = true;
        }
    }
}