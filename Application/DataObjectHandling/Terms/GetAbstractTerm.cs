using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Terms
{
    public class GetAbstractTerm
    {
        public class Query : IRequest<Result<AbstractTermDto>>
        {
            public TermDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<AbstractTermDto>>
        {
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._context = context;
            this._userAccessor = userAccessor;
            }

            public async Task<Result<AbstractTermDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = _userAccessor.GetUsername();
                var profile = await _context.UserLanguageProfiles
                .FirstOrDefaultAsync(p => p.Language == request.Dto.Language && p.User.UserName == _userAccessor.GetUsername());
                return await _context.AbstractTermFor(request.Dto, username);
            }
        }
    }
}