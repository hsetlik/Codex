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
                Console.WriteLine($"Looking for exisitng translation with value {query.QueryValue} for language {query.ResponseLanguage}");
                var matching = await context.Translations
                    .Where(t => t.TermLanguage == query.QueryLanguage && 
                    t.TermValue == query.QueryValue.ToUpper() && 
                    t.UserLanguage == query.ResponseLanguage).ToListAsync();
                if (matching == null || matching.Count < 1)
                {
                    Console.WriteLine("No exisitng translation found!");
                    return Result<TranslatorResponse>.Failure("No matching translations");

                }
                var frequencies = new Dictionary<string, int>();
                foreach(var match in matching)
                {
                    if (frequencies.ContainsKey(match.UserValue))
                        ++frequencies[match.UserValue];
                    else 
                        frequencies[match.UserValue] = 1;
                }
                var tList = frequencies.OrderBy(p => p.Key).ToList();
                var output = new TranslatorResponse
                {
                    Query = query,
                    ResponseValue = tList[0].Key
                };
                Console.WriteLine("Used existing translation!");
                return Result<TranslatorResponse>.Success(output);
            }
    }

}