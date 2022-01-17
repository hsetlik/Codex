using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content;
using Application.Extensions;
using Application.Interfaces;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.ContentRecords
{
    public class ViewContent
    {
        public class Command : IRequest<Result<Unit>>
        {
            public SectionQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var newViewTime = DateTime.Now;
                var content = await _context.Contents
                .FirstOrDefaultAsync(c => c.ContentUrl == request.Dto.ContentUrl);
                if (content == null)
                    return Result<Unit>.Failure($"Content not found for URL: {request.Dto.ContentUrl}");
                var historyResult = await _context.ContentHistoryFor(_userAccessor.GetUsername(), request.Dto.ContentUrl);
                var history = (historyResult.IsSuccess) ? historyResult.Value : null;
                if (history == null) {
                    return Result<Unit>.Failure($"Could not get view history. Error message: {historyResult.Error}");
                }
                    
                //check for an existing ViewRecord and remove it if it exists
                var existingViewRecord = await _context.ContentViewRecords.FirstOrDefaultAsync(v => v.ContentHistoryId == history.ContentHistoryId &&
                v.AccessedOn.Date == newViewTime.Date);
                if (existingViewRecord != null)
                    _context.ContentViewRecords.Remove(existingViewRecord);

                var rec = new ContentViewRecord
                {
                    ContentUrl = request.Dto.ContentUrl,
                    ContentHistoryId = history.ContentHistoryId,
                    ContentHistory = history,
                    AccessedOn = newViewTime,
                    LastSectionViewed = request.Dto.Index
                };
                _context.ContentViewRecords.Add(rec);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("Could not update database");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}