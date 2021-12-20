using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Extensions;
using AutoMapper;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class UpdateUserTerm
    {
        public class Command : IRequest<Result<Unit>>
        {
            public UserTermDetailsDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
     
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var userTerm = await _context.UserTerms.Include(u => u.UserLanguageProfile)
                .FirstOrDefaultAsync(u => u.UserTermId == request.Dto.UserTermId);
                if (userTerm == null)
                    return Result<Unit>.Failure("No matching userTerm");
                if (userTerm.Rating < 3 && request.Dto.Rating >= 3)
                {
                    userTerm.UserLanguageProfile.KnownWords = userTerm.UserLanguageProfile.KnownWords + 1;
                }
                else if(userTerm.Rating >= 3 && request.Dto.Rating < 3)
                {
                    userTerm.UserLanguageProfile.KnownWords = userTerm.UserLanguageProfile.KnownWords - 1;
                }
                userTerm = userTerm.UpdatedWith(request.Dto);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("Could not save Changes!");
                // 
                var historyResult = await _context.UpdateHistory(new DomainDTOs.UserLanguageProfile.ProfileIdDto{LanguageProfileId = userTerm.LanguageProfileId});
                if (!historyResult.IsSuccess)
                    return Result<Unit>.Failure($"Could not update history! Error message: {historyResult.Error}");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}