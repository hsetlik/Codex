using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace Application.Parsing.ParsingExtensions
{
    public static class HtmlNodeExtensions
    {
        public static VideoCaptionElement AsTextElement(this HtmlNode node)
        {
            return new VideoCaptionElement
            {
                
            };
        }

        public static HtmlNode WithAbsoluteImagePaths(this HtmlNode root, string contentUrl)
        {
            var imageNodes = root.CssSelect("img").ToList();
            var url = new Uri(contentUrl);
            var urlRoot = new Uri(contentUrl).DnsSafeHost;
            var rootLong = urlRoot.Substring(0, 8);
            var rootShort = urlRoot.Substring(0, 7);
            Console.WriteLine($"Root Begins with: {rootLong}");
            if (!(rootShort == @"http://" || rootLong == @"https://"))
            {
                string shorter = contentUrl.Substring(0, 7);
                string longer = contentUrl.Substring(0, 8);
                string prefix = (shorter == @"http://") ? shorter : longer;
                urlRoot = prefix + urlRoot;
            }
            foreach(var node in imageNodes)
            {
                string relUrl = "";
                var imgSrc = node.GetAttributeValue("src", "null src");
                Console.WriteLine($"Image source is: {imgSrc}");
                if (imgSrc.Substring(0, 4) == "http" || node.PreviousSibling == null)
                    continue;
                if (node.PreviousSibling.Name == "source")
                {
                    Console.WriteLine("Source node found");
                    // for a source node with a single source path
                    var srcNode = node.PreviousSibling;
                    if (srcNode.Attributes.Any(at => at.Name == "src"))
                    {
                        relUrl = srcNode.GetAttributeValue("src", "no src URL");
                    }
                    if (srcNode.Attributes.Any(at => at.Name == "srcset"))
                    {
                        var strChunks = srcNode.GetAttributeValue("srcset", "no srcset").Split(null).ToList();
                        relUrl = strChunks.FirstOrDefault(c => c.Substring(0, 1) == @"/");
                    }
                }
                var fullUrl = urlRoot + relUrl;
                Console.WriteLine($"replacing {imgSrc} with {fullUrl}");
                node.SetAttributeValue("src", fullUrl);
            }
            return root;
        }
    }
}