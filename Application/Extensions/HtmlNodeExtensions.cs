using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using HtmlAgilityPack;

namespace Application.Extensions
{
    public static class HtmlNodeExtensions
    {
        public static Result<string> ContentUnderHeaderWiki(this HtmlNode spanNode)
        {
            string output = "";
            var headerNode = spanNode.ParentNode;
            var sibling = headerNode.NextSibling;
            while(!sibling.HasClass("h2"))
            {
                if (sibling.Name == "p")
                {
                    output += sibling.InnerText;
                    Console.Write($"Adding inner paragraph with text: {sibling.InnerText}\n");
                }
                var previous = sibling;
                sibling = sibling.NextSibling;
                if (sibling == null)
                {
                    string reportStr = $"Node is null following node with name: {previous.Name} and body text: {previous.InnerText}";
                    return Result<string>.Failure(reportStr);
                }
            }
            return Result<string>.Success(output);
        }
        
    }
}