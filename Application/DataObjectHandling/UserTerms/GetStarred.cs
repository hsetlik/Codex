using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.UserLanguageProfile;
using Application.Extensions;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class GetStarred
    {
        public class Query : IRequest<Result<List<UserTermDetailsDto>>>
        {
            public ProfileIdQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserTermDetailsDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<UserTermDetailsDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GetStarred(request.Dto.LanguageProfileId);
            }
        }
    }
}