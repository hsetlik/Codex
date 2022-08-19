using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.Interfaces;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class UserTermDelete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public TermDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._userAccessor = userAccessor;
            this._context = context;

            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var profile = await _context.UserLanguageProfiles
                .Include(u => u.User)
                .FirstOrDefaultAsync(p => p.Language == request.Dto.Language && p.User.UserName == _userAccessor.GetUsername());
                if (profile == null)
                    return Result<Unit>.Failure("Language profile not found");
                var userTerm = await _context.UserTerms
                .FirstOrDefaultAsync(t => t.LanguageProfileId == profile.LanguageProfileId &&
                t.TermValue == request.Dto.TermValue.AsTermValue());
                if (userTerm == null)
                    return Result<Unit>.Failure("No matching UserTerm found");
                _context.UserTerms.Remove(userTerm);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("UserTerm could not be deleted");
                return Result<Unit>.Success(Unit.Value); 
            }
        }
    }
}