using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.Interfaces;
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
            public UserTermCreateDto termCreateDto { get; set; }
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
                    u => u.Term.Value == request.termCreateDto.TermValue &&
                    u.UserLanguageProfile.UserId == user.Id);
                if (exists) return Result<Unit>.Failure("Term already exists!");
                // 1. Get the ULP by selecting based on UserName and Language
                var profile = await _context.UserLanguageProfiles
                .FirstOrDefaultAsync(
                    x => x.User.UserName == _userAccessor.GetUsername() &&
                    x.Language == request.termCreateDto.Language);
                // 2. Get the term based on the value
                var term = await _context.Terms.FirstOrDefaultAsync(x => x.Value == request.termCreateDto.TermValue);
                // TODO: change this such that if no term exists, one is created
                if (term == null)
                {
                    return Result<Unit>.Failure("No Corresponding term exists!");
                } 
                var currentDateTime = DateTime.Now.ToString();

                var userTerm = new UserTerm
                {
                    LanguageProfileId = profile.LanguageProfileId,
                    UserLanguageProfile = profile,
                    TermId = term.TermId,
                    Term = term,
                    Translations = 
                    { new Translation
                        {
                            TranslationId = Guid.NewGuid(),
                            Value = request.termCreateDto.FirstTranslation,
                            TermId = term.TermId
                        }
                    },
                    TimesSeen = 0,
                    EaseFactor = 2.5f,
                    Rating = 0,
                    DateTimeDue = currentDateTime,
                    SrsIntervalDays = 0,
                    UserTermId = Guid.NewGuid()
                };

                _context.UserTerms.Add(userTerm);
                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Could not create term");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}