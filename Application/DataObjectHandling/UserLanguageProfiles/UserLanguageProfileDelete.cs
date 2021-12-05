using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserLanguageProfiles
{
    public class UserLanguageProfileDelete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public LanguageNameDto Dto { get; set; }
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
                var profile = await _context.UserLanguageProfiles.Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Language == request.Dto.Language && _userAccessor.GetUsername() == p.User.UserName);
                if (profile == null)
                    return Result<Unit>.Failure("Profile not found");
                _context.UserLanguageProfiles.Remove(profile);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure($"Could not remove profile with user: {_userAccessor.GetUsername()} and language: {request.Dto.Language}");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}