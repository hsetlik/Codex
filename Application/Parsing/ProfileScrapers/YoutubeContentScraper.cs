using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Google.Apis.YouTube.v3;

namespace Application.Parsing.ProfileScrapers
{
    public class YoutubeContentScraper : AbstractScraper
    {
        public YoutubeContentScraper(string url) : base(url)
        {
        }

        public override List<ContentSection> GetAllSections()
        {
            throw new NotImplementedException();
        }

        public override ContentMetadataDto GetMetadata()
        {
            throw new NotImplementedException();
        }

        public override int GetNumSections()
        {
            throw new NotImplementedException();
        }

        public override ContentSection GetSection(int index)
        {
            throw new NotImplementedException();
        }

        public override Task PrepareAsync()
        {
            throw new NotImplementedException();
            var service = new YouTubeService();
            var caption = new CaptionsResource(service);
        
        }
    }
}