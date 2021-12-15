using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Parsing.ContentStorage;
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
            var elements = new List<ArticleElement>();
            foreach(var node in uNodes)
            {
                if (node.Name == "p")
                {
                    //Console.WriteLine($"Found <p> element with InnerText: {node.InnerText}");
                    elements.Add(new ArticleElement
                    {
                        Value = node.InnerText,
                        ElementType = ArticleElementType.BodyParagraph
                    });
                }
                else if (node.Name == "span")
                {
                    //Console.WriteLine($"Found <span> element with InnerText: {node.InnerText}");
                    elements.Add(new ArticleElement
                    {
                        Value = node.InnerText,
                        ElementType =  (node.InnerText.Length < 65)? ArticleElementType.Subhead : ArticleElementType.BlockQuote
                    });
                }
                else if (node.Name == "h1")
                {
                    //Console.WriteLine($"Found <h1> element with InnerText: {node.InnerText}");
                    elements.Add(new ArticleElement
                    {
                        Value = node.InnerText,
                        ElementType =  ArticleElementType.Headline
                    });
                }
                else if (node.Name == "h2")
                {
                    //Console.WriteLine($"Found <h2> element with InnerText: {node.InnerText}");
                    elements.Add(new ArticleElement
                    {
                        Value = node.InnerText,
                        ElementType =  ArticleElementType.Subhead
                    });
                }
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
            storage.Elements = elements;
            var sections = storage.GetSections();
            Console.WriteLine($"Article has {sections.Count} sections");
            storage.Sections = sections;
            storage.Metadata.NumSections = sections.Count;
            contentsLoaded = true;
        }
    }

}