using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content.Responses;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class SavedContentContextExtensions
    {
        public static async Task<Result<Unit>> SaveContent(this DataContext context, string contentUrl, Guid langProfileId)
        {
            var profile = await context.UserLanguageProfiles.Include(i => i.SavedContents).FirstOrDefaultAsync(p => p.LanguageProfileId == langProfileId);
            if (profile == null)
                return Result<Unit>.Failure("Could not find profile!");
            var newSavedContent = new SavedContent
            {
                SavedAt = DateTime.Now.ToUniversalTime(),
                ContentUrl = contentUrl,
                LanguageProfileId = langProfileId,
                UserLanguageProfile = profile
            };

            profile.SavedContents.Add(newSavedContent);

            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes!");
            return Result<Unit>.Success(Unit.Value);
        }
        public static async Task<Result<Unit>> UnsaveContent(this DataContext context, string contentUrl, Guid langProfileId)
        {
            var savedContent = await context.SavedContents
                .FirstOrDefaultAsync(c => c.LanguageProfileId == langProfileId &&
                c.ContentUrl == contentUrl);
            if (savedContent == null)
                return Result<Unit>.Failure("Mo matching saved content!");

            context.SavedContents.Remove(savedContent);

            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes!");
            return Result<Unit>.Success(Unit.Value);
        }
        //TODO GetSavedContents(languageProfileId)
        public static async Task<Result<List<SavedContentDto>>> GetSavedContents(this DataContext context, Guid langProfileId)
        {
            var profile = await context
                .UserLanguageProfiles
                .Include(p => p.SavedContents)
                .FirstOrDefaultAsync(p => p.LanguageProfileId == langProfileId);

            if (profile == null)
                return Result<List<SavedContentDto>>.Failure("Could not load profile!");
            var output = new List<SavedContentDto>();
            var mapper = MapperFactory.GetDefaultMapper();
            foreach(var content in profile.SavedContents)
            {
                output.Add(mapper.Map<SavedContentDto>(content));
            }

            return Result<List<SavedContentDto>>.Success(output);
        }
        
    }
}