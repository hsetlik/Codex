using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs.ProfileHistory;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.ProfileHistory
{
    public class GetUserTermsFromDate
    {
        public class Query : IRequest<Result<List<UserTermDetailsDto>>>
        {
            public LanguageDateQuery Dto { get; set; }
        }


        public class Handler : IRequestHandler<Query, Result<List<UserTermDetailsDto>>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<List<UserTermDetailsDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.UserTermsCreatedAt(request.Dto.Language, _userAccessor.GetUsername(), request.Dto.DateQuery);
            }
        }
    }
}