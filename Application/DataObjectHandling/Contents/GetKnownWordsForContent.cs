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
        private readonly IDataRepository _factory;
            public Handler(IDataRepository factory, IUserAccessor userAccessor, IParserService parser)
            {
            this._factory = factory;
            this._parser = parser;
            this._userAccessor = userAccessor;
            }

            public async Task<Result<KnownWordsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var cResult = await _factory.GetMetadataFor(_userAccessor.GetUsername(), request.ContentId);
                if (!cResult.IsSuccess)
                    return Result<KnownWordsDto>.Failure("Could not load content");
                //Console.WriteLine($"Loading content metatata took {watch.ElapsedMilliseconds} ms");
                var scraper = await _parser.GetScraper(cResult.Value.ContentUrl);
                if (scraper == null)
                    return Result<KnownWordsDto>.Failure("Could not get scraper!");
                Console.WriteLine($"GETTING KNOWN WORDS FOR {scraper.GetMetadata().ContentName}. . .");
                //Console.WriteLine($"Getting scraper took {watch.ElapsedMilliseconds} ms");
                var profileResult = await _factory.ProfileFor(_userAccessor.GetUsername(), cResult.Value.Language);
                if (!profileResult.IsSuccess)
                    return Result<KnownWordsDto>.Failure("could not load profile");
                var profile = profileResult.Value;
                var lists = scraper.GetWordLists();
                watch.Stop();
                Console.WriteLine($"Getting {lists.Count} lists took {watch.ElapsedMilliseconds} ms on thread {Thread.CurrentThread.ManagedThreadId}");
                watch.Restart();
                var listData = new List<KnownWordsDto>();
                var listTasks = new List<Task<Result<KnownWordsDto>>>();
                foreach(var list in lists)
                {
                    listTasks.Add(_factory.KnownWordsForList(list, profile.LanguageProfileId));
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
                watch.Stop();
                Console.WriteLine($"FINISHED KNOWN WORDS FOR {scraper.GetMetadata().ContentName} AFTER {watch.ElapsedMilliseconds} ms- {known} Known of {total} total");
                return Result<KnownWordsDto>.Success(new KnownWordsDto
                {
                    KnownWords = known,
                    TotalWords = total
                });
            }
        }
    }
}