using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserLanguageProfiles
{
    public class AllUserTerms
    {
        public class Query : IRequest<Result<List<UserTermDto>>>
        {
            public LanguageNameDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserTermDto>>>
        {
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._context = context;
            this._userAccessor = userAccessor;
            }

            public async Task<Result<List<UserTermDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profile = await _context.UserLanguageProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(u => u.Language == request.Dto.Language &&
                 u.User.UserName == _userAccessor.GetUsername());
                if (profile == null)
                    return Result<List<UserTermDto>>.Failure($"Profile not found for user {_userAccessor.GetUsername()} and language {request.Dto.Language}");
                var output = new List<UserTermDto>();
                var userTerms = await _context.UserTerms
                .Include(u => u.Translations)
                .Include(u => u.Term)
                .Where(t => t.LanguageProfileId == profile.LanguageProfileId)
                .ToListAsync();
                if (userTerms == null)
                    return Result<List<UserTermDto>>.Failure("Could not load user terms");
                foreach(var t in userTerms)
                {
                    var newTerm = t.GetDto();
                    newTerm.Value = t.Term.NormalizedValue;
                    Console.WriteLine($"User term value is: {newTerm.Value}");
                    output.Add(newTerm);
                }
                return Result<List<UserTermDto>>.Success(output);
            }
        }
    }
}