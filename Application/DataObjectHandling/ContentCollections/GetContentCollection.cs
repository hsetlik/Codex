using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.ContentCollection.Responses;
using MediatR;
using Persistence;
using Application.Extensions;
using Application.DomainDTOs.ContentCollection.Queries;

namespace Application.DataObjectHandling.ContentCollections
{
    public class GetContentCollection
    {
        public class Query : IRequest<Result<ContentCollectionDto>>
        {
            public CollectionIdQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentCollectionDto>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<ContentCollectionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GetCollection(request.Dto.ContentCollectionId);
            }
        }
    }
}