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
                var user = await _context.Users
                .Include(u => u.UserLanguageProfiles)
                .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());
                if (user == null)
                    return Result<Unit>.Failure("Could not find user");
                user.UserLanguageProfiles = user.UserLanguageProfiles.Where(p => p.Language != request.Dto.Language).ToList();
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure($"Could not remove profile with user: {_userAccessor.GetUsername()} and language: {request.Dto.Language}");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}