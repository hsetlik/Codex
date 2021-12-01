using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Parsing.ProfileParsers;

namespace Application.Parsing
{
    public static class ContentParserFactory
    {
        public static HtmlContentParser ParserFor(string url)
        {
            var profile = HtmlContentParser.ProfileFor(url);
            Console.WriteLine($"Creating Parser for: {url} with profile {profile.Value}");
            if (profile.Value == ParserProfile.Wikipedia.Value)
            {
                Console.WriteLine("Creating Wikipedia parser....");
                return new WikipediaContentParser(url);
            }
            else
            {
                return new NewsArticleContentParser(url);
            }
        }
        
    }
}