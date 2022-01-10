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
using AutoMapper;
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
        private readonly DataContext _context;
        private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
            this._mapper = mapper;
            this._context = context;
            }

            public async Task<Result<ContentMetadataDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var contentEntity = await _context.Contents.Include(c => c.ContentTags).FirstOrDefaultAsync(c => c.ContentUrl == request.Dto.ContentUrl);
                if (contentEntity == null)
                    return Result<ContentMetadataDto>.Failure("Could not find matching content");
                var output = _mapper.Map<ContentMetadataDto>(contentEntity);
                return Result<ContentMetadataDto>.Success(output);
            }
        }
    }
}