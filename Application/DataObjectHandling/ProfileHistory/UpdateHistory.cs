using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.UserLanguageProfile;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.ProfileHistory
{
    public class UpdateHistory
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ProfileIdDto Dto { get; set; }
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
                var profile = await _context.UserLanguageProfiles
                    .Include(p => p.DailyProfileHistory)
                    .FirstOrDefaultAsync(p => p.LanguageProfileId == request.Dto.LanguageProfileId);
                if (profile == null)
                    return Result<Unit>.Failure($"Could not get profile: {request.Dto.LanguageProfileId}");

                // check for an existing record from today
                var existingRecord = await _context.DailyProfileRecords
                .FirstOrDefaultAsync(r => r.LanguageProfileId == request.Dto.LanguageProfileId && r.CreatedAt.Date == DateTime.Now.Date);
                // if we find one, remove it from the context
                if(existingRecord != null)
                    _context.DailyProfileRecords.Remove(existingRecord);
                
                
                var record = new DailyProfileRecord
                {
                    DailyProfileHistoryId = profile.DailyProfileHistory.DailyProfileHistoryId,
                    DailyProfileHistory = profile.DailyProfileHistory,
                    LanguageProfileId = request.Dto.LanguageProfileId,
                    UserLanguageProfile = profile,
                    CreatedAt = DateTime.Now,
                    KnownWords = profile.KnownWords
                };
                _context.DailyProfileRecords.Add(record);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure($"Could not save changes!");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}