using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
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
        public class Query : IRequest<Result<List<UserLanguageProfileDto>>>
        {
            
        }

        public class Handler : IRequestHandler<Query, Result<List<UserLanguageProfileDto>>>
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
            public async Task<Result<List<UserLanguageProfileDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                
                var profiles = user.UserLanguageProfiles;
                var profileDtos = new List<UserLanguageProfileDto>();
                foreach(var profile in profiles)
                {
                    profileDtos.Add(
                        new UserLanguageProfileDto
                    {
                        UserId = profile.UserId,
                        Language = profile.Language
                    });
                }
                return Result<List<UserLanguageProfileDto>>.Success(profileDtos);

            }
        }
    }
}