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
                var lists = scraper.GetWordLists();
                watch.Stop();
                Console.WriteLine($"Getting {lists.Count} lists took {watch.ElapsedMilliseconds} ms on thread {Thread.CurrentThread.ManagedThreadId}");
                watch.Start();
                int known = 0;
                int total = 0;
                Parallel.ForEach(lists, async list => 
                {
                    int idx = lists.IndexOf(list);
                    Console.WriteLine($"List {idx} has {list.Count} words");
                    var w = System.Diagnostics.Stopwatch.StartNew();
                   
                    var results =  await _factory.KnownWordsForList(list, profileResult.Value.LanguageProfileId);
                    if (results.IsSuccess)
                    {
                        known += results.Value.KnownWords;
                        total += list.Count;
                    }
                    else
                        Console.WriteLine($"Failed with error message: {results.Error}");
                    w.Stop();
                    
                    Console.WriteLine($"Parsing list {idx} took {w.ElapsedMilliseconds} ms on thread {Thread.CurrentThread.ManagedThreadId}. List had {results.Value.KnownWords} of {results.Value.TotalWords}");
                });
                watch.Stop();
                Console.WriteLine($"FINISHED KNOWN WORDS FOR {scraper.GetMetadata().ContentName} AFTER {watch.ElapsedMilliseconds} ms");
                return Result<KnownWordsDto>.Success(new KnownWordsDto
                {
                    KnownWords = known,
                    TotalWords = total
                });
            }
        }
    }
}