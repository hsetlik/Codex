using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.Interfaces;
using Application.Utilities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class TranslationContextExtensions
    {
        public static async Task<Result<List<TranslationResultDto>>> ListPopularTranslations(this DataContext context, TermDto term)
        {
            var userTerms = await context.UserTerms
                .Include(u => u.Translations)
                .Where(u => u.NormalizedTermValue == term.Value.AsTermValue() &&
                u.Language == term.Language)
                .ToListAsync();
            
                if (userTerms == null)
                {
                    return Result<List<TranslationResultDto>>.Failure("could not load matching userTerms");
                }
                // 3. Go through each translation and add them to a dictionary
                var translationFrequencies = new Dictionary<string, int>();
                foreach (var userTerm in userTerms)
                {
                    foreach(var translation in userTerm.Translations)
                    {
                        var tValue = translation.UserValue;
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
                var output = new List<TranslationResultDto>();
                foreach(var kvp in translationFrequencies)
                {
                    var popTranslation = new TranslationResultDto(kvp.Key, kvp.Value);
                    output.Add(popTranslation);
                }
                return Result<List<TranslationResultDto>>.Success(output);
        }

        public static async Task<Result<List<TranslationResultDto>>> GetTranslationsAsync(this DataContext context, ITranslator translator, TermDto term, string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return Result<List<TranslationResultDto>>.Failure($"Could not load user with name {username}");
            var googleResult = await translator.GetTranslation(new DomainDTOs.Translator.TranslatorQuery
            {
                ResponseLanguage = user.NativeLanguage,
                QueryLanguage = term.Language,
                QueryValue = term.Value
            });
            if (!googleResult.IsSuccess)
                return Result<List<TranslationResultDto>>.Failure($"Failed to get google result! Error message: {googleResult.Error}");
            var output = new List<TranslationResultDto>();
            output.Add(new TranslationResultDto(googleResult.Value.ResponseValue));
            var popTranslationsResult = await context.ListPopularTranslations(term);
            if (!popTranslationsResult.IsSuccess)
                output.AddRange(popTranslationsResult.Value);
            return Result<List<TranslationResultDto>>.Success(output);
        }
        
    }
}