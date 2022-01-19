using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Feed.FeedRows
{
    public class MostViewedRow : AbstractFeedRow
    {
        public MostViewedRow(Guid id) : base(id)
        {
        }

        public override async Task<Result<List<ContentMetadataDto>>> GetContents(DataContext context, int max, IMapper mapper)
        {
            var profile = await context.UserLanguageProfiles.FirstOrDefaultAsync(p => p.LanguageProfileId == languageProfileId);
            if (profile == null)
                return Result<List<ContentMetadataDto>>.Failure($"No profile with ID {languageProfileId}");
            var urls = await context.ContentViewRecords.Select(r => r.ContentUrl).ToListAsync();
            if (urls == null)
                return Result<List<ContentMetadataDto>>.Failure($"No valid contents found");
            var urlFrequencyTable = new Dictionary<string, int>();
            foreach(var url in urls)
            {
                if (urlFrequencyTable.Any(p => p.Key == url))
                {
                    urlFrequencyTable[url] = urlFrequencyTable[url] + 1;
                }
                else
                    urlFrequencyTable[url] = 1;
            }
            var mostViewed = urlFrequencyTable.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).Take(max).ToList();
            
            throw new NotImplementedException();
        }
    }
}