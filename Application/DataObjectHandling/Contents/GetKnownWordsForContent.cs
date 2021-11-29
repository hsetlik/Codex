using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.UserLanguageProfile;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetKnownWordsForContent
    {
        public class Query : IRequest<Result<KnownWordsDto>>
        {
           public Guid ContentId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<KnownWordsDto>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<KnownWordsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GetKnownWords(request.ContentId, _userAccessor.GetUsername());
            }
        }
    }
}