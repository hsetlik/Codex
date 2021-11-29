using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content;
using Application.Extensions;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetContentTags
    {
        public class Query : IRequest<Result<List<ContentTagDto>>>
        {
            public ContentIdDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ContentTagDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<ContentTagDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var list =  await _context.GetContentTags(request.Dto.ContentId);
                if (!list.IsSuccess)
                    return Result<List<ContentTagDto>>.Failure("no matching tags found");
                return list;
            }
        }
    }
}