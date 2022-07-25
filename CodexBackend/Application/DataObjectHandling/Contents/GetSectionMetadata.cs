using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetSectionMetadata
    {
        public class Query : IRequest<Result<ContentSectionDto>>
        {
            public SectionQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentSectionDto>>
        {
        private readonly DataContext _context;
        private readonly IParserService _parser;
            public Handler(DataContext context, IParserService parser)
            {
            this._parser = parser;
            this._context = context;
            }

            public async Task<Result<ContentSectionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var content = await _context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == request.Dto.ContentUrl);
                if (content == null)
                    return Result<ContentSectionDto>.Failure($"Could not load content with URL: {request.Dto.ContentUrl}");
                var section = await _parser.GetSection(content.ContentUrl, request.Dto.Index);
                if (section == null)
                    return Result<ContentSectionDto>.Failure("Could not load section");
                var output = new ContentSectionDto
                {
                    ContentUrl = content.ContentUrl,
                    SectionHeader = section.SectionHeader,
                    Index = request.Dto.Index,
                    NumElements = section.TextElements.Count
                };
                return Result<ContentSectionDto>.Success(output);
            }
        }
    }
}