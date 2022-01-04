using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.ContentCollection.Queries;
using Application.DomainDTOs.ContentCollection.Responses;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class ContentCollectionContextExtensions
    {
        /*
            Functions needed: 
            1-4. CRUD functions
        */
        public static async Task<Result<Unit>> CreateCollection(this DataContext context, CreateCollectionDto dto)
        {
            var profile = await context.UserLanguageProfiles
                .Include(p => p.CreatedCollections)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.LanguageProfileId == dto.CreatorProfileId);
            if (profile == null)
                return Result<Unit>.Failure("Could not load profile!");
            Console.WriteLine($"Profile with ID {profile.LanguageProfileId} loaded");
            var content = await context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == dto.FirstContentUrl);
            if (content == null)
                return Result<Unit>.Failure($"Could not load content at {dto.FirstContentUrl}");
            
            var collection = new ContentCollection
            {
                LanguageProfileId = dto.CreatorProfileId,
                CreatorUsername = profile.User.UserName,
                Language = profile.Language,
                CollectionName = dto.CollectionName,
                Description = dto.Description,
                CreatedAt = DateTime.Now,
                Entries = new List<ContentCollectionEntry>
                {
                    new ContentCollectionEntry 
                    {
                        ContentId = content.ContentId,
                        Content = content,
                        LanguageProfileId = dto.CreatorProfileId,
                        UserLanguageProfile = profile,
                        AddedAt = DateTime.Now
                    }
                }
            };

            profile.CreatedCollections.Add(collection);

            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes");

             var saveSuccess = await context.SaveCollection(new SaveCollectionQuery
            {
                ContentCollectionId = collection.ContentCollectionId,
                LanguageProfileId = collection.LanguageProfileId
            });

            if (!saveSuccess.IsSuccess)
                return Result<Unit>.Failure("Could not save collection!");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<ContentCollectionDto>> GetCollection(this DataContext context, Guid collectionId)
        {
            var collection = await context.ContentCollections
                .Include(c => c.Entries)
                .ThenInclude(c => c.Content)
                .FirstOrDefaultAsync(c => c.ContentCollectionId == collectionId);
            if (collection == null)
                return Result<ContentCollectionDto>.Failure($"Collection with ID: {collectionId}");
            var output = MapperFactory.GetDefaultMapper().Map<ContentCollectionDto>(collection);
            return Result<ContentCollectionDto>.Success(output);
        }

        public static async Task<Result<Unit>> UpdateCollection(this DataContext context, ContentCollectionDto dto)
        {
            var existing = await context.ContentCollections
                .Include(c => c.Entries)
                .FirstOrDefaultAsync(c => c.ContentCollectionId == dto.ContentCollectionId);
            if (existing == null)
                return Result<Unit>.Failure("No existing collection!");
            var mapper = MapperFactory.GetDefaultMapper();

            foreach(var entry in existing.Entries)
            {
                foreach(var content in dto.CollectionContents)
                {
                    if (!dto.CollectionContents.Any(c => c.ContentId == entry.ContentId))
                    {
                        //remove from collection
                        context.ContentCollectionEntries.Remove(entry);
                        var entryRemoved = await context.SaveChangesAsync() > 0;
                        if (!entryRemoved)
                            return Result<Unit>.Failure("Could not save changes!");
                    }
                    else if (!existing.Entries.Any(c => c.ContentId == content.ContentId))
                    {
                        // add to collection
                        var addResult = await context.AddToCollection(new AddToCollectionQuery
                        {
                            ContentCollectionId = existing.ContentCollectionId,
                            ContentUrl = content.ContentUrl
                        });
                        if(!addResult.IsSuccess)
                            return Result<Unit>.Failure($"Failed to add result! Error message: {addResult.Error}");
                    }
                }
            }

            var updated = mapper.Map<ContentCollection>(dto);
            context.ContentCollections.Remove(existing);
            context.ContentCollections.Add(updated);

            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes!");

            // need to add this to SavedContents
            var saveSuccess = await context.SaveCollection(new SaveCollectionQuery
            {
                ContentCollectionId = updated.ContentCollectionId,
                LanguageProfileId = updated.LanguageProfileId
            });
            if (!saveSuccess.IsSuccess)
                return Result<Unit>.Failure($"Failed to save collection! Error message: {saveSuccess.Error}");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> DeleteCollection(this DataContext context, Guid collectionId)
        {
            Console.WriteLine($"Preparing to delete collection with ID: {collectionId}");
            var existing = await context.ContentCollections.FirstOrDefaultAsync(c => c.ContentCollectionId == collectionId);
            if (existing == null)
                return Result<Unit>.Failure("No existing Collection!");
            Console.WriteLine($"Existing collection loaded with name {existing.CollectionName}");
            context.ContentCollections.Remove(existing);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes!");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> AddToCollection(this DataContext context, AddToCollectionQuery query)
        {
            var collection = await context.ContentCollections
                .Include(c => c.UserLanguageProfile)
                .FirstOrDefaultAsync(c => c.ContentCollectionId == query.ContentCollectionId);
            if (collection == null)
                return Result<Unit>.Failure($"Could not get collection with ID {query.ContentCollectionId}");
            var content = await context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == query.ContentUrl);
            if (content == null)
                return Result<Unit>.Failure($"Could not find content with URL: {query.ContentUrl}");
            var entry = new ContentCollectionEntry 
            {
                ContentId = content.ContentId,
                Content = content,
                LanguageProfileId = collection.LanguageProfileId,
                UserLanguageProfile = collection.UserLanguageProfile,
                AddedAt = DateTime.Now
            };
            context.ContentCollectionEntries.Add(entry);
            var success = await context.SaveChangesAsync() > 0;
            if(!success)
                return Result<Unit>.Failure("Could not save changes!");
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> SaveCollection(this DataContext context, SaveCollectionQuery query)
        {
            var collection = await context.ContentCollections
                .Include(c => c.UserLanguageProfile)
                .FirstOrDefaultAsync(c => c.ContentCollectionId == query.ContentCollectionId);
            if (collection == null)
                return Result<Unit>.Failure($"Could not get collection with ID {query.ContentCollectionId}");
            var profile = await context.UserLanguageProfiles
                .Include(p => p.SavedCollections)
                .ThenInclude(c => c.ContentCollection)
                .FirstOrDefaultAsync(p => p.LanguageProfileId == query.LanguageProfileId);
            if (profile == null)
                return Result<Unit>.Failure($"Could not get profile with ID {query.LanguageProfileId}");

            // check to make sure this collection hasn't already been saved
            if (profile.SavedCollections.Any(c => c.ContentCollectionId == collection.ContentCollectionId))
                return Result<Unit>.Failure($"Collection with ID {collection.ContentCollectionId} is already saved!");
            var savedCollection = new SavedContentCollection
            {
                LanguageProfileId = profile.LanguageProfileId,
                UserLanguageProfile = profile,
                ContentCollectionId = collection.ContentCollectionId,
                ContentCollection = collection,
                SavedAt = DateTime.Now
            };

            context.SavedContentCollections.Add(savedCollection);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes!");
            
            return Result<Unit>.Success(Unit.Value);
        }



    }
}