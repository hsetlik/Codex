using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.ProfileHistory;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.ProfileHistory
{
    public class GetMetricGraph
    {
        public class Query : IRequest<Result<MetricGraph>>
        {
            public MetricGraphQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<MetricGraph>>
        {
        private readonly DataContext _context;
        private readonly IProfileHistoryEngine _history;
            public Handler(DataContext context, IProfileHistoryEngine history)
            {
            this._history = history;
            this._context = context;
            }

            public async Task<Result<MetricGraph>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _history.GetMetricGraph(request.Dto, _context);
            }
        }
    }
}