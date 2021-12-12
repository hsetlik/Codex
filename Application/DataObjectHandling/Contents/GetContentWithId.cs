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
        private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<ContentMetadataDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var contentResult = await _context.GetMetadataFor(_userAccessor.GetUsername(), request.Dto.ContentId);
                if (!contentResult.IsSuccess)
                    return Result<ContentMetadataDto>.Failure($"Failed to get metadata! Error message{contentResult.Error}");
                return Result<ContentMetadataDto>.Success(contentResult.Value);
            }
        }
    }
}