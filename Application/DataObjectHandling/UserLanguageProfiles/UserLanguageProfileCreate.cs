using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserLanguageProfiles
{
    public class UserLanguageProfileCreate
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string LanguageId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _mapper = mapper;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                //1. grab the current user from the IdentityDbContext subclass
                var user = await _context.Users.Include(u => u.UserLanguageProfiles).FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

                //2. create the lang profile
                var langProfile = new UserLanguageProfile
                {
                    UserId = user.Id,
                    User = user,
                    Language = request.LanguageId,
                    KnownWords = 0
                };
                //3. check if the user already has a profile for this language
                var profileExists = user.UserLanguageProfiles.Contains(langProfile);
                //4. if the profile exists, return failure
                if (profileExists) return Result<Unit>.Failure("User already has a profile for this language");
                //5. otherwise, add it to the user object
                user.UserLanguageProfiles.Add(langProfile);
                //6. use AutoMapper to map the user back onto the DataContext
                //6. update the database
                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Creating language profile failed");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}