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
            var profile = AbstractScraper.ProfileFor(url);
            if (profile.Value == ScraperProfile.Wikipedia.Value)
            {
                return new WikipediaContentScraper(url);
            }
            else if (profile.Value == ScraperProfile.Youtube.Value)
            {
                return new YoutubeContentScraper(url);
            }
            else
            {
                return new NewsArticleContentScraper(url);
            }
        }
        
    }
}