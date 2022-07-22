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
    public class NewestRow : FeedRowGenerator
    {
        public NewestRow(Guid id) : base(id)
        {
        }

        public override async Task<Result<List<ContentMetadataDto>>> GetContentList(DataContext context, int max, IMapper mapper)
        {
            var profile = await context.UserLanguageProfiles.FirstOrDefaultAsync(c => c.LanguageProfileId == languageProfileId);
            if (profile == null)
                return Result<List<ContentMetadataDto>>.Failure($"No profile with ID {languageProfileId}");
            var contents = await context.Contents
                .Include(c => c.ContentTags)
                .Where(c => c.Language == profile.Language)
                .ToListAsync();
            if (contents == null)
                return Result<List<ContentMetadataDto>>.Failure($"No contents with language {profile.Language} after");
            contents = contents.OrderByDescending(c => c.CreatedAt).Take(max).ToList();
            return Result<List<ContentMetadataDto>>.Success(contents.Select(c => mapper.Map<ContentMetadataDto>(c)).ToList());
        }
    }
}