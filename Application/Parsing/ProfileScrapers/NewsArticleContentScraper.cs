using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Parsing.ContentStorage;
using Application.Parsing.ParsingExtensions;
using HtmlAgilityPack;
using MediatR;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

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

        public override ContentSection GetSection(int index)
        {
            return storage.Sections[index];
        }

        public override async Task PrepareAsync()
        {
            //  get the HTML
            var downloader = new HttpDownloader(this.Url, null, null);
            var pageText = await Task.Run(() => downloader.GetPage()); 
            var document = new HtmlDocument();
            document.LoadHtml(pageText);
            var root = document.DocumentNode;
            var headlineNode = root.CssSelect("h1").FirstOrDefault();
            string headline = (headlineNode == null) ? "headline not found" : headlineNode.InnerText;
            // create a list of nodes, just grab anything inside relevant HTML tags
            var lang = root.GetAttributeValue("lang", "language not found");
            if (lang == "language not found")
            {
                var htmlNode = root.CssSelect("html").FirstOrDefault();
                if (htmlNode != null)
                {
                    lang = htmlNode.GetAttributeValue("lang", "language not found");
                }
            }
            var uNodes = new List<HtmlNode>();
            uNodes.Add(headlineNode);
            uNodes.AddRange(root.CssSelect("p").ToList());
            uNodes.AddRange(root.CssSelect("span").ToList());
            uNodes.AddRange(root.CssSelect("h1").ToList());
            uNodes.AddRange(root.CssSelect("h2").ToList());
            var nodes = uNodes.OrderBy(n => n.Line).ToList();
            var elements = new List<TextElement>();
            foreach(var node in uNodes)
            {
                elements.Add(node.AsTextElement());
            }
            
            storage.Metadata = new ContentMetadataDto
            {
                ContentUrl = this.Url,
                ContentName = headline,
                ContentType = "Article",
                Language = lang,
                Bookmark = 0,
                VideoUrl = "none",
                AudioUrl = "none",
                NumSections = 0
            };
            var sections = new List<ContentSection>();
            // now to parse the elements into sections
            var currentSection = new ContentSection
            {
                ContentUrl = Url,
                Index = sections.Count,
                SectionHeader = headline,
                TextElements = new List<TextElement>()
            };
            foreach(var element in elements)
            {
                if ((element.Tag == "h1" || element.Tag == "h2") && currentSection.TextElements.Count < 1)
                {
                    currentSection.SectionHeader = element.Value;
                }
                currentSection.TextElements.Add(element);
                if (currentSection.Body.Split(null).Count() > 300)
                {
                    sections.Add(currentSection);
                    currentSection = new ContentSection
                    {
                        ContentUrl = Url,
                        Index = sections.Count,
                        SectionHeader = headline,
                        TextElements = new List<TextElement>()
                    };
                }
            }

            storage.Elements = elements;
            Console.WriteLine($"Article has {sections.Count} sections");
            storage.Sections = sections;
            storage.Metadata.NumSections = sections.Count;
            contentsLoaded = true;
        }
    }

}