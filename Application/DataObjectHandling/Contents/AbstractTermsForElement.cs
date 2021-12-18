using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs.Content;
using Application.Interfaces;
using MediatR;

namespace Application.DataObjectHandling.Contents
{
    public class AbstractTermsForElement
    {
        public class Query : IRequest<Result<ElementAbstractTerms>>
        {
            public ElementTermsQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ElementAbstractTerms>>
        {
        private readonly IDataRepository _factory;
        private readonly IUserAccessor _userAccessor;
        private readonly IParserService _parser;
            public Handler(IDataRepository factory, IUserAccessor userAccessor, IParserService parser)
            {
            this._parser = parser;
            this._userAccessor = userAccessor;
            this._factory = factory;
            }

            public async Task<Result<ElementAbstractTerms>> Handle(Query request, CancellationToken cancellationToken)
            {
                var content = await _parser.GetContentMetadata(request.Dto.ContentUrl);
                var section = await _parser.GetSection(request.Dto.ContentUrl, request.Dto.SectionIndex);
                if (section == null)
                    return Result<ElementAbstractTerms>.Failure("Could not load section");
                var element = section.TextElements[request.Dto.ElementIndex];
                var terms = new List<AbstractTermDto>();
                var words = element.Value.Split(null).ToList();
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
                    Index = request.Dto.ElementIndex,
                    Tag = element.Tag,
                    AbstractTerms = orderedTerms
                };
                return Result<ElementAbstractTerms>.Success(output);
            }
        }
    }
}