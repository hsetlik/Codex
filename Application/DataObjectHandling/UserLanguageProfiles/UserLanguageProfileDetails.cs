using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.UserLanguageProfile;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserLanguageProfiles
{
    public class UserLanguageProfileDetails
    {
        public class Query : IRequest<Result<LanguageProfileDto>>
        {
            public ProfileIdDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<LanguageProfileDto>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<LanguageProfileDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profile = await _context.UserLanguageProfiles
                .FirstOrDefaultAsync(p => p.LanguageProfileId == request.Dto.LanguageProfileId);
                if (profile == null)
                    return Result<LanguageProfileDto>.Failure("No profile found");
                var output = new LanguageProfileDto
                {
                    LanguageProfileId = profile.LanguageProfileId,
                    Language = profile.Language,
                    KnownWords = profile.KnownWords
                };
                return Result<LanguageProfileDto>.Success(output);

            }
        }
    }
}