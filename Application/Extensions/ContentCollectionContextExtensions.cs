using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
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
        public static async Task<Result<Unit>> CreateCollection(this DataContext context, ContentCollectionDto dto)
        {
            var profile = await context.UserLanguageProfiles
                .Include(c => c.ContentCollections)
                .FirstOrDefaultAsync(c => c.LanguageProfileId == dto.LanguageProfileId);
            var mapper = MapperFactory.GetDefaultMapper();
            if (profile == null)
                return Result<Unit>.Failure("Profile not found");

            var contents = await context.Contents.Where(c => dto.ContentMetadataDtos.Any(m => m.ContentUrl == c.ContentUrl)).ToListAsync();
       
            var collection = new ContentCollection
            {
                UserLanguageProfile = profile,
                LanguageProfileId = profile.LanguageProfileId,
                Language = profile.Language,
                CreatedAt = DateTime.Now,
                Contents = contents,
                CollectionName = dto.CollectionName
            };

            context.ContentCollections.Add(collection);

            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Could not save updates!");
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
                .Include(c => c.Contents)
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