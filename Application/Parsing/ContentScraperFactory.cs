using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Parsing;
using Application.Parsing.ProfileScrapers;

namespace Application.Parsing
{
    public static class ContentScraperFactory
    {
        public static AbstractScraper ScraperFor(string url)
        {
            Console.WriteLine($"Getting profile for {url}");
            var profile = AbstractScraper.ProfileFor(url);
            Console.WriteLine($"Creating Parser for: {url} with profile {profile.Value}");
            if (profile.Value == ScraperProfile.Wikipedia.Value)
            {
                Console.WriteLine("Creating Wikipedia parser....");
                return new WikipediaContentScraper(url);
            }
            else
            {
                return new NewsArticleContentScraper(url);
            }
        }
        
    }
}