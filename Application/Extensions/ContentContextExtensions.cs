using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using AutoMapper;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class ContentContextExtensions
    {
         public static async Task<Result<Unit>> AddContentTag(this DataContext context, ContentTagDto dto)
         {
            var content = await context.Contents
            .Include(c => c.ContentTags)
            .FirstOrDefaultAsync(t => t.ContentId == dto.ContentId);
            if(content == null)
                return Result<Unit>.Failure("No matching content");
            var tag = new ContentTag
            {
                Content = content,
                ContentId = content.ContentId,
                TagValue = dto.TagValue
            };
            content.ContentTags.Add(tag);
            var success = await context.SaveChangesAsync() > 0;
            if (! success)
                return Result<Unit>.Failure("Could not save changes");
            return Result<Unit>.Success(Unit.Value);
         }

        public static async Task<Result<List<ContentTagDto>>> GetContentTags(this DataContext context, Guid contentId)
        {
            var output = new List<ContentTagDto>();
            var tags = await context.ContentTags.Where(tag => tag.ContentId == contentId).ToListAsync();
            if (tags == null)
                return Result<List<ContentTagDto>>.Failure("No matching tags found");
            foreach(var tag in tags)
            {
                output.Add(new ContentTagDto
                {
                    ContentId = contentId,
                    TagValue = tag.TagValue
                });
            }
            return Result<List<ContentTagDto>>.Success(output);
        }

        public static async Task<Result<List<ContentMetadataDto>>> GetContentsWithTag(this DataContext context, string tagValue)
        {
            var contents = await context.ContentTags
            .Include(u => u.Content)
            .Where(u => u.TagValue == tagValue)
            .ToListAsync();
            if (contents == null)
                return Result<List<ContentMetadataDto>>.Failure("Could not find matching tags");
            var dict = new Dictionary<string, ContentMetadataDto>();
            foreach(var tag in contents)
            {
                var dto = new ContentMetadataDto 
                {
                    VideoUrl = tag.Content.VideoUrl ,
                    AudioUrl = tag.Content.AudioUrl,
                    ContentType = tag.Content.ContentType,
                    ContentName = tag.Content.ContentName,
                    Language = tag.Content.Language,
                    ContentUrl = tag.Content.ContentUrl,
                    ContentId = tag.ContentId
                };
                dict[tag.TagValue] = dto;
            }
            var list = dict.Values.ToList();
            return Result<List<ContentMetadataDto>>.Success(list);
        }

        public static async Task<Result<DomainDTOs.ContentMetadataDto>> GetContentAtUrl(this DataContext context, string url, IMapper mapper)
        {
            var content = await context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == url);
            
            if (content == null)
                return Result<DomainDTOs.ContentMetadataDto>.Failure("Could not load content");
            var value = mapper.Map<DomainDTOs.ContentMetadataDto>(content);
            return Result<DomainDTOs.ContentMetadataDto>.Success(value);
        }
    }
}