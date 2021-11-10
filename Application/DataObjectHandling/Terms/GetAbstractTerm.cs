using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
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
                var term = await _context.Terms
                .FirstOrDefaultAsync(
                    x => x.Language == request.Dto.Language && 
                    x.Value == request.Dto.Value);
                if (term == null) return Result<AbstractTermDto>.Failure("No valid term found!");
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                if (user == null) return Result<AbstractTermDto>.Failure("User not found!");
                TermDto termDto;
                var userTerm = await _context.UserTerms
                .Include(u => u.Translations)
                .Include(u => u.UserLanguageProfile)
                .FirstOrDefaultAsync(x => x.TermId == term.TermId && x.UserLanguageProfile.UserId == user.Id);
                //whether the userterm exists determines which subclass is created
                if (userTerm != null)
                {
                    //we have a userterm, so we create the UserTermDto subclass
                    var translations = new List<string>();
                    foreach(var t in userTerm.Translations) //NOTE: there's definitely a better C#-ish way to do this
                    {
                        translations.Add(t.Value);
                    }
                    termDto = new UserTermDto
                    {
                        Value = term.Value,
                        Language = term.Language,
                        HasUserTerm = true,
                        EaseFactor = userTerm.EaseFactor,
                        SrsIntervalDays = userTerm.SrsIntervalDays,
                        Rating = userTerm.Rating,
                        Translations = translations
                    };
                }
                else
                {
                    termDto = new RawTermDto
                    {
                        Value = term.Value,
                        Language = term.Language
                    };
                }
                var aTermDto = AbstractTermFactory.Generate(termDto);
                return Result<AbstractTermDto>.Success(aTermDto);
            }
        }
    }
}