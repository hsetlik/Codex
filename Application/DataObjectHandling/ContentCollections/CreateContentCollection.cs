using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.ContentCollection.Queries;
using Application.DomainDTOs.ContentCollection.Responses;
using Application.Extensions;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.ContentCollections
{
    public class CreateContentCollection
    {
        public class Command : IRequest<Result<Unit>>
        {
            public CreateCollectionDto Dto { get; set; }
        }


        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _context.CreateCollection(request.Dto);
            }
        }
    }
}