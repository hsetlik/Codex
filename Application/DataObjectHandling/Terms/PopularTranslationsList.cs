using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Terms
{
    public class PopularTranslationsList
    {
        public class Query : IRequest<Result<List<PopTranslationDto>>>
        {
            public TermDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<PopTranslationDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<PopTranslationDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // 1. Find the term
                var term = await _context.Terms.FirstOrDefaultAsync(u => u.NormalizedValue == request.Dto.Value && u.Language == request.Dto.Language); 
                if (term == null) return Result<List<PopTranslationDto>>.Failure("No valid term found");
                
                // 2. grab all corresponding UserTerms and their translations
                var userTerms = await _context.UserTerms
                .Include(u => u.Translations)
                .Where(u => u.TermId == term.TermId)
                .ToListAsync();

                // 3. Go through each translation and add them to a dictionary
                var translationFrequencies = new Dictionary<string, int>();
                foreach (var userTerm in userTerms)
                {
                    foreach(var translation in userTerm.Translations)
                    {
                        var tValue = translation.Value;
                        if (translationFrequencies.ContainsKey(tValue)) //increment frequency value if the translation already exists
                        {
                            translationFrequencies[tValue] = translationFrequencies[tValue] + 1;
                        }
                        else
                        { //otherwise add a new translations with a count of one
                            translationFrequencies.Add(tValue, 1);
                        }
                    }
                }
                
                // 4. Convert each KVP into a translation DTO and return the list
                var output = new List<PopTranslationDto>();
                foreach(var kvp in translationFrequencies)
                {
                    var popTranslation = new PopTranslationDto
                    {
                        Value = kvp.Key,
                        NumInstances = kvp.Value
                    };
                    output.Add(popTranslation);
                }
                return Result<List<PopTranslationDto>>.Success(output);
            }
        }

    }
}