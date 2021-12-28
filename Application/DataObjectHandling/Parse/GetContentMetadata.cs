using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Parse
{
    public class GetContentMetadata
    {
        public class Query : IRequest<Result<ContentMetadataDto>>
        {
            public ContentUrlQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentMetadataDto>>
        {
        private readonly IParserService _parser;
        private readonly DataContext _context;
            public Handler(IParserService parser, DataContext context)
            {
            this._context = context;
            this._parser = parser;
            }

            public async Task<Result<ContentMetadataDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var contentEntity = await _context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == request.Dto.ContentUrl);
                if (contentEntity == null)
                    return Result<ContentMetadataDto>.Failure("Could not find matching content");
                var output = contentEntity.GetMetadata();
                return Result<ContentMetadataDto>.Success(output);
            }
        }
    }
}