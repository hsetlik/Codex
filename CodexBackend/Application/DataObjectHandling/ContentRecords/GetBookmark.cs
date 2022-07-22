using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.ContentRecords
{
    public class GetBookmark
    {
        public class Query : IRequest<Result<int>>
        {
            public ContentUrlQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                var existingRecord = await _context.ContentViewRecords
                .Include(c => c.ContentHistory)
                .ThenInclude(h => h.UserLanguageProfile)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(c => c.ContentUrl == request.Dto.ContentUrl 
                && c.ContentHistory.UserLanguageProfile.User.UserName == _userAccessor.GetUsername());
                if (existingRecord == null)
                    return Result<int>.Success(0);
                return Result<int>.Success(existingRecord.LastSectionViewed);
            }
        }
    }
}