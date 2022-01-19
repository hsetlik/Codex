using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FeedObjects.FeedRows
{
    public class MostViewedRow : FeedRowGenerator
    {
        public MostViewedRow(Guid id) : base(id)
        {
        }

        public override async Task<Result<List<ContentMetadataDto>>> GetContentList(DataContext context, int max, IMapper mapper)
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
            var mostViewedUrls = urlFrequencyTable.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).Take(max).ToList();
            var output = new List<ContentMetadataDto>();
            foreach(var url in mostViewedUrls)
            {
                var content = await context.Contents.Include(c => c.ContentTags).FirstOrDefaultAsync(c => c.ContentUrl == url);
                if (content == null)
                    return Result<List<ContentMetadataDto>>.Failure($"Could not load content with URL: {url}");
                output.Add(mapper.Map<ContentMetadataDto>(content));
            }
            return Result<List<ContentMetadataDto>>.Success(output);
        }
    }
}