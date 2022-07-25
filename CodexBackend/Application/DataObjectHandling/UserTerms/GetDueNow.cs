using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class GetDueNow
    {
        public class Command : IRequest<Result<List<UserTermDetailsDto>>>
        {
            public LanguageNameQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<List<UserTermDetailsDto>>>
        {
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._context = context;
            this._userAccessor = userAccessor;
            }
            
            public  async Task<Result<List<UserTermDetailsDto>>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _context.UserTermsDueNow(request.Dto, _userAccessor.GetUsername());
            }
        }
    }
}