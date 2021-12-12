using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using MediatR;

namespace Application.Parsing.ProfileScrapers
{
       // NewsArticle
    public class NewsArticleContentScraper : AbstractScraper
    {
        public NewsArticleContentScraper(string url) : base(url)
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
        }
    }

}