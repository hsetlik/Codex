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
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<ContentCollectionDto>> GetCollection(this DataContext context, Guid collectionId)
        {
            var collection = await context.ContentCollections.FirstOrDefaultAsync(c => c.ContentCollectionId == collectionId);
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
            var updated = mapper.Map<ContentCollection>(dto);
            context.ContentCollections.Remove(existing);
            context.ContentCollections.Add(updated);

            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes!");
            
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> DeleteCollection(this DataContext context, Guid collectionId)
        {
            var existing = await context.ContentCollections.FirstOrDefaultAsync(c => c.ContentCollectionId == collectionId);
            if (existing == null)
                return Result<Unit>.Failure("No existing Collection!");
            context.ContentCollections.Remove(existing);
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save changes!");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}