using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.UserLanguageProfile;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetKnownWordsForContent
    {
        public class Query : IRequest<Result<KnownWordsDto>>
        {
           public Guid ContentId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<KnownWordsDto>>
        {
        private readonly IUserAccessor _userAccessor;
        private readonly IParserService _parser;

        private readonly DataContext _context;
            public Handler(DataContext context, IUserAccessor userAccessor, IParserService parser)
            {
            this._parser = parser;
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<KnownWordsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var cResult = await _context.GetMetadataFor(_userAccessor.GetUsername(), request.ContentId);
                if (!cResult.IsSuccess)
                    return Result<KnownWordsDto>.Failure("Could not load content");
                var scraper = await _parser.GetScraper(cResult.Value.ContentUrl);
                if (scraper == null)
                    return Result<KnownWordsDto>.Failure("Could not get scraper!");
                var profileResult = await _context.GetProfileAsync(_userAccessor.GetUsername(), cResult.Value.Language);
                if (!profileResult.IsSuccess)
                    return Result<KnownWordsDto>.Failure("could not load profile");
                var profile = profileResult.Value;
                var lists = scraper.GetWordLists();
                var listData = new List<KnownWordsDto>();
                var listTasks = new List<Task<Result<KnownWordsDto>>>();
                foreach(var list in lists)
                {
                    listTasks.Add(_context.KnownWordsForListAsync(list, profile.LanguageProfileId));
                }
                int known = 0;
                int total = 0;
                var listResults = await Task.WhenAll(listTasks);
                foreach(var result in listResults)
                {
                    if (result.IsSuccess)
                    {
                        known += result.Value.KnownWords;
                        total += result.Value.TotalWords;
                    }
                }
                return Result<KnownWordsDto>.Success(new KnownWordsDto
                {
                    KnownWords = known,
                    TotalWords = total
                });
            }
        }
    }
}