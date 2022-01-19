using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Application.FeedObjects;
using Application.DomainDTOs.Feed.Queries;
using System.Threading;
using Persistence;
using AutoMapper;

namespace Application.DataObjectHandling.FeedHandling
{
    public class GetFeed
    {
        public class Query : IRequest<Result<Feed>>
        {
            public FeedQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Feed>>
        {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
            public Handler(DataContext context, IMapper mapper)
            {
            this._context = context;
            this._mapper = mapper;
            }

            public async Task<Result<Feed>> Handle(Query request, CancellationToken cancellationToken)
            {
                var output = new Feed
                {
                    LanguageProfileId = request.Dto.LanguageProfileId,
                    Rows = new List<FeedRow>()
                };
                var feedTypes = Enum.GetNames<FeedType>();
                var generators = Enum.GetValues<FeedType>()
                    .Select(t => RowFactory.RowFor(t, request.Dto.LanguageProfileId))
                    .ToList();
                for(int i = 0; i < generators.Count; ++i)
                {
                    var listResult = await generators[i].GetContentList(_context, 5, _mapper);
                    if (!listResult.IsSuccess)
                    {
                        return Result<Feed>.Failure($"Could not get content list! Error Message: {listResult.Error}");
                    }
                    output.Rows.Add(new FeedRow
                    {
                        FeedType = feedTypes[i],
                        Contents = listResult.Value
                    });
                }
                return Result<Feed>.Success(output);
            }
        }
    }
}