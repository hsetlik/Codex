using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Extensions;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class UpdateUserTerm
    {
        public class Command : IRequest<Result<Unit>>
        {
            public UserTermDetailsDto Dto { get; set; }
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
                var userTerm = await _context.UserTerms.FindAsync(request.Dto.UserTermId);
                if (userTerm == null)
                    return Result<Unit>.Failure("No matching userTerm");
                userTerm = userTerm.UpdatedWith(request.Dto);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("Could not update term!");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}