using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content;
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
            public ContentUrlQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Unit>>
        {
        private readonly IParserService _parser;
        private readonly DataContext _context;
            public Handler(DataContext context, IParserService parser)
            {
            this._context = context;
            this._parser = parser;
            }

            public async Task<Result<Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                var existingContent = await _context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == request.Dto.ContentUrl);
                if (existingContent != null)
                    return Result<Unit>.Failure("Content at this URL is already in database");
                Console.WriteLine($"Content not found for URL {request.Dto.ContentUrl}. Attempting to create...");
                var metadata = await _parser.GetContentMetadata(request.Dto.ContentUrl);
                if (metadata == null)
                    return Result<Unit>.Failure("Could not create metadata");
                var date = DateTime.Now.ToString();
                var content = new Content
                {
                    ContentUrl = metadata.ContentUrl,
                    ContentName = metadata.ContentName,
                    ContentType = metadata.ContentType,
                    VideoUrl = metadata.VideoUrl,
                    Language = metadata.Language,
                    DateAdded = date,
                    ContentTags = new List<ContentTag>(),
                    NumSections = metadata.NumSections
                };
                _context.Contents.Add(content);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("Could not save changes");
                return Result<Unit>.Success(Unit.Value); 
            }
        }
    }
}