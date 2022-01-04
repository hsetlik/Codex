using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Collection.Queries;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class CollectionExtensions
    {
        public static async Task<Result<Unit>> CreateCollection(this DataContext context, CreateCollectionQuery query)
        {
            var profile = await context.UserLanguageProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Language == query.Language && p.User.UserName == query.CreatorUserName);
            if (profile == null)
                return Result<Unit>.Failure("No profile found");
            var firstContent = await context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == query.FirstContentUrl);
            if (firstContent == null)
                return Result<Unit>.Failure($"No content found at URL {query.FirstContentUrl}");
            var collection = new Collection
            {
                CreatorLanguageProfileId = profile.LanguageProfileId,
                CreatorUserName = query.CreatorUserName,
                IsPrivate = query.IsPrivate,
                CreatedAt = DateTime.Now,
                Language = profile.Language,
                CollectionName = query.CollectionName,
                Description = query.Description,
                CollectionMembers = new List<CollectionMember>
                {
                    new CollectionMember
                    {
                        Content = firstContent,
                        ContentId = firstContent.ContentId
                    }
                }
            };
            context.Collections.Add(collection);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes");
            return Result<Unit>.Success(Unit.Value);
        }
        
    }
}