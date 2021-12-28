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
        //====
        public static async Task<Result<Unit>> AddContentTag(this DataContext context, ContentTagQuery dto)
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


        //===
        public static async Task<Result<List<ContentTagQuery>>> GetContentTags(this DataContext context, Guid contentId)
        {
            var output = new List<ContentTagQuery>();
            var tags = await context.ContentTags.Where(tag => tag.ContentId == contentId).ToListAsync();
            if (tags == null)
                return Result<List<ContentTagQuery>>.Failure("No matching tags found");
            foreach(var tag in tags)
            {
                output.Add(new ContentTagQuery
                {
                    ContentId = contentId,
                    TagValue = tag.TagValue
                });
            }
            return Result<List<ContentTagQuery>>.Success(output);
        }

        //===
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

        // get the metadata without the bookmark (no username passed)
        //===
        public static async Task<Result<ContentMetadataDto>> GetMetadataFor(this DataContext context, string url)
        {
            var content = await context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == url);
            
            if (content == null)
                return Result<DomainDTOs.ContentMetadataDto>.Failure("Could not load content");
            

            return Result<ContentMetadataDto>.Success(new ContentMetadataDto
            {
                ContentName = content.ContentName,
                ContentType = content.ContentType,
                ContentUrl = content.ContentUrl,
                Language = content.Language,
                VideoUrl = content.VideoUrl,
                AudioUrl = content.AudioUrl,
                ContentId = content.ContentId,
                Bookmark = 0,
                NumSections = content.NumSections
                
            });
        }
        // get the metadata WITH the bookmark
        //=====
        public static async Task<Result<ContentMetadataDto>> GetMetadataFor(this DataContext context, string username, string url)
        {
            var metadataResult = await context.GetMetadataFor(url);
            if (!metadataResult.IsSuccess)
                return Result<ContentMetadataDto>.Failure($"Failed to get content metadata. Error Message: {metadataResult.Error}");
            var metadata = metadataResult.Value;
            var record = await context.LatestRecordFor(username, url);
            // if we have a record, update the bookmark
            if(record.IsSuccess)
                metadata.Bookmark = record.Value.LastSectionViewed;

            return Result<ContentMetadataDto>.Success(metadata);
        }

        //===
        public static async Task<Result<ContentMetadataDto>> GetMetadataFor(this DataContext context, string username, Guid contentId)
        {
            var content = await context.Contents.FindAsync(contentId);
            if (content == null)
                return Result<ContentMetadataDto>.Failure($"Could not load content with ID: {contentId}");
            var url = content.ContentUrl;
            var metadataResult = await context.GetMetadataFor(url);
            if (!metadataResult.IsSuccess)
                return Result<ContentMetadataDto>.Failure($"Failed to get content metadata. Error Message: {metadataResult.Error}");
            var metadata = metadataResult.Value;
            var record = await context.LatestRecordFor(username, url);
            // if we have a record, update the bookmark
            if(record.IsSuccess)
                metadata.Bookmark = record.Value.LastSectionViewed;
            return Result<ContentMetadataDto>.Success(metadata);
        }

        // functionality for GetLanguageContents.cs

        public static async Task<Result<List<ContentMetadataDto>>> GetContentsForLanguage(this DataContext context, string username, string lang)
        {
            var langUrls = await context.Contents.Where(c => c.Language == lang).ToListAsync();
            if (langUrls == null)
                return Result<List<ContentMetadataDto>>.Failure($"Could not load contents for language: {lang}");
            var output = new List<ContentMetadataDto>();
            foreach(var url in langUrls)
            {
                var metadata = await context.GetMetadataFor(username, url.ContentUrl);
                if (!metadata.IsSuccess)
                    return Result<List<ContentMetadataDto>>.Failure($"Failed to load metadata! Error message: {metadata.Error}");
                output.Add(metadata.Value);
            }
            return Result<List<ContentMetadataDto>>.Success(output);
        }
  
    }
}