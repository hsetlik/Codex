using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using HtmlAgilityPack;
using MediatR;

namespace Application.DataObjectHandling.Contents
{
    public class ContentParser
    {
        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }
        private static HtmlDocument ToHtmlDoc(string fullText)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(fullText);
            return doc;
        }
        public static async Task<Result<ContentCreateDto>> ParseToContent(string url)
        {
            var html = await CallUrl(url);
            var htmlDoc = ToHtmlDoc(html);
            var paragraphs = htmlDoc.DocumentNode.Descendants("p").ToList();
            var htmlNode = htmlDoc.DocumentNode.Descendants("html").FirstOrDefault();
            var lang = htmlNode.GetAttributeValue("lang", "en");
            string fullText = "";
            foreach(var paragraph in paragraphs)
            {
                Console.Write($"Paragraph {paragraphs.IndexOf(paragraph)}: \n {paragraph.InnerText}");
                fullText += paragraph.InnerText + '\n';
            }
            var headNode = htmlDoc.DocumentNode.Descendants("head").FirstOrDefault();

            return Result<ContentCreateDto>.Success(new ContentCreateDto
            {
                ContentName = $"{headNode.Descendants("title").FirstOrDefault().InnerText}",
                ContentType = "Article",
                Language = lang,
                FullText = fullText,
                AudioUrl = "none",
                VideoUrl = "none"
            });
        }

        public class Query : IRequest<Result<ContentCreateDto>>
        {
            public string Url { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentCreateDto>>
        {
            public Handler()
            {
            }

            public async Task<Result<ContentCreateDto>> Handle(Query request, CancellationToken cancellationToken)
            {
               return await ParseToContent(request.Url);
            }
        }
    }
}