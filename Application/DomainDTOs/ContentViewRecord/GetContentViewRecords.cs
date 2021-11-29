using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Extensions;
using MediatR;
using Persistence;

namespace Application.DomainDTOs.ContentViewRecord
{
    public class GetContentViewRecords
    {
        public class Query : IRequest<Result<List<ContentViewRecordDto>>>
        {
            public Guid ContentId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ContentViewRecordDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<ContentViewRecordDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var list = await _context.AllViewRecords(request.ContentId);
                if (list == null)
                    return Result<List<ContentViewRecordDto>>.Failure("No matching records found");
                return Result<List<ContentViewRecordDto>>.Success(list.Value);
            }
        }
    }
}