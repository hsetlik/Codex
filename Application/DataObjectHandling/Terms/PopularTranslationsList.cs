using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

//TODO: Fix this lol
public class TranslationDto
{
    public string TermValue { get; set; }
  
}

public class PopTranslationDto
{
    public string Value { get; set; }
    public int NumInstances { get; set; }
}
namespace Application.DataObjectHandling.Terms
{
    public class PopularTranslationsList
    {
        public class Query : IRequest<Result<List<PopTranslationDto>>>
        {
            public TranslationDto TranslationDto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<PopTranslationDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public Task<Result<List<PopTranslationDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
                /*
                var term = await _context.Terms.FirstOrDefaultAsync(x => x.Value == request.TranslationDto.TermValue);
                var translations = await _context.Translations
                .Include(t => t.UserTerm)
                .ThenInclude(t => t.Term)
                .Where(x => x.TermId == term.TermId)
                .ToListAsync();
                Console.WriteLine(request.TranslationDto.TermValue);
                Dictionary<string, int> popTranslations = new Dictionary<string, int>{{"str", 0}};
                
                foreach(var value in translations)
                {
                    Debug.WriteLine(value);
                    if (!popTranslations.ContainsKey(value.Value))
                    {
                        popTranslations[value.Value] = 1;
                    }
                    else
                    {
                        popTranslations[value.Value] += 1;
                    }
                }
                var list = new List<PopTranslationDto>();
                foreach(var pair in popTranslations)
                {
                    list.Add( new PopTranslationDto
                    {
                        Value = pair.Key,
                        NumInstances = pair.Value
                    });
                }
                return Result<List<PopTranslationDto>>.Success(list);
                */
            }
        }

    }
}