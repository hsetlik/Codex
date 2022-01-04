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
    public static class CollectionContextExtensions
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
                CollectionMembers = new List<CollectionContent>
                {
                    new CollectionContent
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
            // now save this collection to the profile
            var saveResult = await context.SaveCollection(new SavedCollectionQuery
            {
                CollectionId = collection.CollectionId,
                LanguageProfileId = profile.LanguageProfileId
            });

            if (!saveResult.IsSuccess)
                return Result<Unit>.Failure($"Could not save collection! Error message: {saveResult.Error}");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> DeleteCollection(this DataContext context, CollectionIdQuery query)
        {
            var existing = await context.Collections.FirstOrDefaultAsync(c => c.CollectionId == query.CollectionId);
            if (existing == null)
                return Result<Unit>.Failure($"No collection with ID {query.CollectionId}");
            context.Collections.Remove(existing);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("could not save changes");

            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> SaveCollection(this DataContext context, SavedCollectionQuery query)
        {
            var collection = await context.Collections.FindAsync(query.CollectionId);
            if (collection == null)
                return Result<Unit>.Failure($"No collection with ID {query.CollectionId}");
            var profile = await context.UserLanguageProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.LanguageProfileId == query.LanguageProfileId);
            if (profile == null)
                return Result<Unit>.Failure($"No profile found with ID {query.LanguageProfileId}");
            bool isOwner = collection.CreatorUserName == profile.User.UserName;

            var savedCollection = new SavedCollection
            {
                CollectionId = collection.CollectionId,
                Collection = collection,
                LanguageProfileId = profile.LanguageProfileId,
                UserLanguageProfile = profile,
                IsOwner = isOwner,
                SavedAt = DateTime.Now
            };

            context.SavedCollections.Add(savedCollection);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes");
            return Result<Unit>.Success(Unit.Value);
        } 


        
    }
}