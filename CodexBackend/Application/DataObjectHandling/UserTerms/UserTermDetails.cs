using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.Interfaces;
using Application.Utilities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class UserTermDetails
    {
        public class Query : IRequest<Result<UserTermDetailsDto>>
        {
            public TermDto TermDto { get; set; }

        }
        public class Handler : IRequestHandler<Query, Result<UserTermDetailsDto>>
        {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
            this._userAccessor = userAccessor;
            this._mapper = mapper;
            this._context = context;
            }

            public async Task<Result<UserTermDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                string queryTerm = "";
                try
                {
                    queryTerm = request.TermDto.TermValue;
                    Console.WriteLine($"Query word is: {queryTerm}");
                }
                catch (System.Exception exc)
                {
                    return Result<UserTermDetailsDto>.Failure($"Could not get user term! Exception: {exc.Message}");
                }
                var user = await _context.Users.Include(u => u.UserLanguageProfiles).FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                var profile = await _context.UserLanguageProfiles.FirstOrDefaultAsync(
                    x => x.UserId == user.Id &&
                    x.Language == request.TermDto.Language);
                if (profile == null) return Result<UserTermDetailsDto>.Failure("No associated profile found");
                var profileId = profile.LanguageProfileId;
                var parsedTerm = request.TermDto.TermValue.Normalize().ToUpper(); 
                Console.WriteLine($"Parsed term is: {parsedTerm}");
                var userTerm = await _context.UserTerms.FirstOrDefaultAsync(
                    x => x.LanguageProfileId == profileId &&
                    x.TermValue == parsedTerm);
                if (userTerm == null) return Result<UserTermDetailsDto>.Failure("No associated user term found");
                var dto = new UserTermDetailsDto
                {
                    NormalizedTermValue = request.TermDto.TermValue,
                    TimesSeen = userTerm.TimesSeen,
                    EaseFactor = userTerm.EaseFactor,
                    Rating = userTerm.Rating,
                    DateTimeDue = userTerm.DateTimeDue,
                    SrsIntervalDays = userTerm.SrsIntervalDays,
                    UserTermId = userTerm.UserTermId
                };
                return Result<UserTermDetailsDto>.Success(dto);
                
            }
        }
    }
}