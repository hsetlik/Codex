using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Utilities;
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
                var userTerms = await _context.UserTerms
                .Include(t => t.Term)
                .Include(u => u.Translations)
                .Where(u => u.Term.NormalizedValue == request.Dto.Value.AsTermValue() &&
                u.Term.Language == request.Dto.Language)
                .ToListAsync();
                

                if (userTerms == null)
                {
                    return Result<List<PopTranslationDto>>.Failure("could not load matching userTerms");
                }
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