using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetContentWithId
    {
        public class Query : IRequest<Result<ContentMetadataDto>>
        {
            public ContentIdDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentMetadataDto>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<ContentMetadataDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var content =  await _context.Contents.FirstOrDefaultAsync(c => c.ContentId == request.Dto.ContentId);
                if (content == null)
                    return Result<ContentMetadataDto>.Failure("content not loaded");
                var output = content.GetMetadata();
                return Result<ContentMetadataDto>.Success(output);
            }
        }
    }
}