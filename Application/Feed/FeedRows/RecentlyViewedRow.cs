using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using AutoMapper;
using Domain.DataObjects;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.FeedObjects.FeedRows
{
    public class RecentlyViewedRow : FeedRowGenerator
    {
        public RecentlyViewedRow(Guid id) : base(id)
        {
        }

        public override async Task<Result<List<ContentMetadataDto>>> GetContentList(DataContext context, int max, IMapper mapper)
        {
            var profile = await context.UserLanguageProfiles
                .Include(p => p.ContentHistories)
                .FirstOrDefaultAsync(t => t.LanguageProfileId == languageProfileId);
            if (profile == null)
                return Result<List<ContentMetadataDto>>.Failure($"No profile with ID {languageProfileId}");
            var historyIds = profile.ContentHistories.Select(h => h.ContentHistoryId).ToList();
            var rangeBeginning = DateTime.Now.AddDays(-30.0);
            var recordsInRange = await context.ContentViewRecords
                .Where(r => r.AccessedOn >= rangeBeginning && 
                historyIds.Any(id => id == r.ContentHistoryId))
                .ToListAsync();
            if (recordsInRange == null)
                return Result<List<ContentMetadataDto>>.Failure($"No valid records in after time: {rangeBeginning}");
            recordsInRange = recordsInRange.OrderByDescending(r => r.AccessedOn).Take(max).ToList();
            var uniqueRecords = new List<ContentViewRecord>();
            foreach(var rec in recordsInRange)
            {
                if (!uniqueRecords.Any(r => r.ContentUrl == rec.ContentUrl))
                    uniqueRecords.Add(rec);
            }
            recordsInRange = uniqueRecords.Take(max).ToList();
            var output = new List<ContentMetadataDto>();

            foreach(var record in recordsInRange)
            {
                var content = await context.Contents.Include(c => c.ContentTags).FirstOrDefaultAsync(c => c.ContentUrl == record.ContentUrl);
                if (content == null)
                    return Result<List<ContentMetadataDto>>.Failure($"No content found for URL: {record.ContentUrl}");
                output.Add(mapper.Map<ContentMetadataDto>(content));
            }
            return Result<List<ContentMetadataDto>>.Success(output);
        }
    }
}