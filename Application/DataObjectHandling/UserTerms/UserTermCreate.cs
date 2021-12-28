using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.Interfaces;
using Application.Utilities;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class UserTermCreate
    {
        public class Command : IRequest<Result<Unit>>
        {
            public UserTermCreateQuery termCreateDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly DataContext _context;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                //Check if the UserTerm with this value already exists
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                var exists = await _context.UserTerms.AnyAsync(
                    u => u.NormalizedTermValue == request.termCreateDto.TermValue &&
                    u.UserLanguageProfile.UserId == user.Id);
                if (exists) return Result<Unit>.Failure("Term already exists!");
                // 1. Get the ULP by selecting based on UserName and Language
                string normValue = request.termCreateDto.TermValue.AsTermValue();
                var profile = await _context.UserLanguageProfiles
                .FirstOrDefaultAsync(
                    x => x.User.UserName == _userAccessor.GetUsername() &&
                    x.Language == request.termCreateDto.Language);
                if (profile == null)
                    return Result<Unit>.Failure("No corresponding language profile exists!");
                var userTerm = new UserTerm
                {
                    UserLanguageProfile = profile,
                    NormalizedTermValue = normValue,
                    Translations = new List<Translation>
                    { 
                        new Translation
                        {
                            TermValue = normValue,
                            TermLanguage = profile.Language,
                            UserValue = request.termCreateDto.FirstTranslation,
                            UserLanguage = profile.UserLanguage
                        }
                    },
                    TimesSeen = 0,
                    EaseFactor = 2.5f,
                    Rating = 0,
                    DateTimeDue = DateTime.Now,
                    SrsIntervalDays = 0,
                    CreatedAt = DateTime.Now
                };
                _context.UserTerms.Add(userTerm);
                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Could not create term");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}