using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetContentWithName
    {
        public class Query : IRequest<Result<ContentMetadataDto>>
        {
            public string ContentName { get; set; }
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
                var content =  await _context.Contents.FirstOrDefaultAsync(c => c.ContentName == request.ContentName);
                if (content == null)
                    return Result<ContentMetadataDto>.Failure("content not loaded");
                var output = _mapper.Map<ContentMetadataDto>(content); 
                return Result<ContentMetadataDto>.Success(output);
            }
        }
    }
}