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
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetContentsWithTag
    {
        public class Command : IRequest<Result<List<ContentMetadataDto>>>
        {
            public TagValueDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<List<ContentMetadataDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<ContentMetadataDto>>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _context.GetContentsWithTag(request.Dto.TagValue);
            }
        }
    }
}