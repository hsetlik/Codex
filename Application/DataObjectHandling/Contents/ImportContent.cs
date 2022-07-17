using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.Content.Queries;
using Application.Interfaces;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class ImportContent
    {
        public class Query : IRequest<Result<Unit>>
        {
            public CreateContentQuery Dto { get; set; }
           
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
        private readonly IParserService _parser;
        private readonly DataContext _context;
        private readonly IUserAccessor _user;
            public Handler(DataContext context, IParserService parser, IUserAccessor user)
            {
            this._user = user;
            this._context = context;
            this._parser = parser;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                var existingContent = await _context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == request.Dto.ContentUrl);
                if (existingContent != null)
                    return Result<Unit>.Failure("Content at this URL is already in database");
                var metadata = await _parser.GetContentMetadata(request.Dto.ContentUrl);
                if (metadata == null)
                    return Result<Unit>.Failure("Could not create metadata");
                var profile = await _context.UserLanguageProfiles
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Language == metadata.Language && p.User.UserName == _user.GetUsername());
                if (profile == null)
                    return Result<Unit>.Failure($"User {_user.GetUsername()} has no profile for {metadata.Language}");
                
                var content = new Content
                {
                    ContentUrl = metadata.ContentUrl,
                    ContentName = metadata.ContentName,
                    ContentType = metadata.ContentType,
                    VideoId = metadata.VideoId,
                    Language = metadata.Language,
                    CreatedAt = DateTime.Now.ToUniversalTime(),
                    NumSections = metadata.NumSections,
                    CreatorUsername = _user.GetUsername(),
                    UserLanguageProfile = profile,
                    LanguageProfileId = profile.LanguageProfileId,
                    Description = request.Dto.Description
                };
                content.ContentTags = request.Dto.Tags.Select(t => new ContentTag
                    {
                        Content = content,
                        ContentId = content.ContentId,
                        TagValue = t,
                        TagLanguage = profile.UserLanguage
                    }).ToList();
                _context.Contents.Add(content);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("Could not save changes");
                return Result<Unit>.Success(Unit.Value); 
            }
        }
    }
}