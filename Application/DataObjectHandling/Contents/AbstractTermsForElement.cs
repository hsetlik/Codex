using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.UserTerm.Queries;
using Application.Interfaces;
using Application.Parsing;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    using TaskMap =  Dictionary<int, Task<Result<AbstractTermDto>>>;
    public class AbstractTermsForElement
    {
        public class Query : IRequest<Result<ElementAbstractTerms>>
        {
            public ElementAbstractTermsQuery Dto { get; set; }
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

                var terms = new List<AbstractTermDto>();
                string text = request.Dto.ElementText.WithoutSquareBrackets();
                var words = text.Split(null).ToList();
                words = words.TakeWhile(w => Regex.IsMatch(w, @"[^\s+]")).ToList();
                var wordDict = new Dictionary<int, string>();
                for(int i = 0; i < words.Count; ++i)
                {
                    wordDict[i] = words[i];
                }
                var taskMap = new TaskMap();

                Parallel.ForEach(wordDict, word => 
                {
                    taskMap[word.Key] = _factory.GetAbstractTerm(new TermDto{Value = word.Value, Language = request.Dto.Language}, _userAccessor.GetUsername());
                });
                foreach(var t in taskMap)
                {
                    var term = await t.Value;
                    if (term.IsSuccess)
                    {
                        term.Value.IndexInChunk = t.Key;
                        terms.Add(term.Value);
                    }
                }
                terms = terms.OrderBy(t => t.IndexInChunk).ToList();
                var output = new ElementAbstractTerms
                    {
                    ElementText = request.Dto.ElementText,
                    Tag = request.Dto.Tag,
                    AbstractTerms = terms
                    };
                     return Result<ElementAbstractTerms>.Success(output);     
            }
        }
    }
}