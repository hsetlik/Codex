using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Collection.Queries;
using Application.DomainDTOs.Collection.Responses;
using Application.Extensions;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Collections
{
    public class GetCollection
    {
        public class Query : IRequest<Result<CollectionDto>>
        {
            public CollectionIdQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<CollectionDto>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<CollectionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GetCollection(request.Dto);
            }
        }
    }
}