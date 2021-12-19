using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs.Content;
using Application.Interfaces;
using Application.Parsing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class AbstractTermsForElement
    {
        public class Query : IRequest<Result<ElementAbstractTerms>>
        {
            public TextElement TextElement { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ElementAbstractTerms>>
        {
        private readonly IDataRepository _factory;
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
            public Handler(IDataRepository factory, IUserAccessor userAccessor, DataContext context)
            {
            this._context = context;
            this._userAccessor = userAccessor;
            this._factory = factory;
            }

            public async Task<Result<ElementAbstractTerms>> Handle(Query request, CancellationToken cancellationToken)
            {
                var content = await _context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == request.TextElement.ContentUrl);
                if (content == null)
                    return Result<ElementAbstractTerms>.Failure("Could not load content");
                var terms = new List<AbstractTermDto>();
                var words = request.TextElement.Value.Split(null).ToList();
                var wordDict = new Dictionary<int, string>();
                for(int i = 0; i < words.Count; ++i)
                {
                    wordDict[i] = words[i];
                }
                Parallel.ForEach(wordDict, async word => 
                {
                    var newTermResult = await _factory.GetAbstractTerm(new TermDto{Value = word.Value, Language = content.Language}, _userAccessor.GetUsername());
                    if (newTermResult.IsSuccess)
                    {
                        var newTerm = newTermResult.Value;
                        newTerm.IndexInChunk = word.Key;
                        terms.Add(newTerm);
                        Console.WriteLine($"Term with value {newTerm.TermValue} at index {word.Key}");
                    }
                });
                var orderedTerms = terms.OrderBy(t => t.IndexInChunk).ToList();
                var output = new ElementAbstractTerms
                {
                    Index = request.TextElement.Index,
                    Tag = request.TextElement.Tag,
                    AbstractTerms = orderedTerms
                };
                return Result<ElementAbstractTerms>.Success(output);
            }
        }
    }
}