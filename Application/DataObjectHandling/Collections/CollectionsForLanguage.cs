using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Collection.Queries;
using Application.DomainDTOs.Collection.Responses;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Collections
{
    public class CollectionsForLanguage
    {
        public class Query : IRequest<Result<List<CollectionDto>>>
        {
            public CollectionsForLanguageQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CollectionDto>>>
        {
            private readonly DataContext context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._userAccessor = userAccessor;
                this.context = context;
            }

            public async Task<Result<List<CollectionDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await context.CollectionsForLanguage(request.Dto, _userAccessor);
            }
        }
    }
}