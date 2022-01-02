using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.DomainDTOs.Translator;
using Application.Interfaces;
using Application.Utilities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class TranslationContextExtensions
    {
            public static async Task<Result<TranslatorResponse>> GetTopTranslation(this DataContext context, TranslatorQuery query)
            {
                var matching = await context.Translations
                    .Where(t => t.TermLanguage == query.QueryLanguage && 
                    t.TermValue == query.QueryValue && 
                    t.UserLanguage == query.ResponseLanguage).ToListAsync();
                if (matching == null || matching.Count < 1)
                    return Result<TranslatorResponse>.Failure("No matching translations");
                var frequencies = new Dictionary<string, int>();
                foreach(var match in matching)
                {
                    if (frequencies.ContainsKey(match.UserValue))
                        ++frequencies[match.UserValue];
                    else 
                        frequencies[match.UserValue] = 1;
                }
                var top = frequencies.Max();
                var output = new TranslatorResponse
                {
                    Query = query,
                    ResponseValue = top.Key
                };
                return Result<TranslatorResponse>.Success(output);
            }
    }

}