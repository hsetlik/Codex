using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.ContentHistory;
using Application.DomainDTOs.HistoryQueries;
using Application.Extensions;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.ProfileHistory
{
    public class GetDailyKnownWords
    {
        public class Query : IRequest<Result<List<DailyKnownWordsDto>>>
        {
            public KnownWordsQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<DailyKnownWordsDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<DailyKnownWordsDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var knownWords = new List<DailyKnownWordsDto>();
                for (int i = 0; i < request.Dto.NumDays; ++i)
                {
                    var date = DateTime.Now.AddDays(i * -1).Date;
                    var known = await _context.GetKnownWordsForDay(date, request.Dto.UserLanguageProfileId);
                    if (known.IsSuccess)
                    {
                        knownWords.Add(new DailyKnownWordsDto
                        {
                            Value = known.Value.Value,
                            Date = date
                        });
                    }
                }
                return Result<List<DailyKnownWordsDto>>.Success(knownWords);
            }
        }
    }
}