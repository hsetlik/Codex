using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.UserLanguageProfile;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserLanguageProfiles
{
    public class UserLanguageProfileList
    {
        public class Query : IRequest<Result<List<LanguageProfileDto>>>
        {
            
        }

        public class Handler : IRequestHandler<Query, Result<List<LanguageProfileDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;

            }

            public async Task<Result<List<LanguageProfileDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(x => x.UserLanguageProfiles).FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                
                var profiles = user.UserLanguageProfiles;
                var profileDtos = new List<LanguageProfileDto>();
                foreach(var profile in profiles)
                {
                    profileDtos.Add(
                        new LanguageProfileDto
                    {
                        Language = profile.Language,
                        LanguageProfileId = profile.LanguageProfileId,
                        KnownWords = profile.KnownWords
                    });
                }
                return Result<List<LanguageProfileDto>>.Success(profileDtos);

            }
        }
    }
}