using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Phrase;
using Application.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class StringContextExtensions
    {
        public static async Task<Result<List<PhraseDto>>> GetContainedPhrases(this string text, DataContext context, Guid profileId, IMapper mapper)
        {
            var output = new List<PhraseDto>();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var profile = await context.UserLanguageProfiles
                .Include(p => p.Phrases)
                .FirstOrDefaultAsync(p => p.LanguageProfileId == profileId);
            if (profile == null)
                return Result<List<PhraseDto>>.Failure($"No profile with ID {profileId}");
            watch.Stop();
            Console.WriteLine($"Querying {profile.Phrases.Count} phrases took {watch.ElapsedMilliseconds} ms");
            var textNormTerms = text.SplitToNormalizedTerms();
            var phraseNormTerms = profile.Phrases.ToDictionary(p => p.PhraseId, p => p.Value.SplitToNormalizedTerms());
            Parallel.ForEach(phraseNormTerms, kvp => 
            {
                var firstWord = kvp.Value[0];
                var possibleMatches = new List<List<string>>();
                for(int i = 0; i < textNormTerms.Count; ++i)
                {
                    if (textNormTerms[i] == firstWord)
                        possibleMatches.Add(textNormTerms.GetRange(i, kvp.Value.Count).ToList());
                }
                var match = possibleMatches.FirstOrDefault(strs => strs.SequenceEqual(kvp.Value));
                if (match != null)
                {
                    var phrase = Task.Run<PhraseDto>( async () => 
                    {
                        var phrase = await context.Phrases.FirstOrDefaultAsync(p => p.PhraseId == kvp.Key);
                        return mapper.Map<PhraseDto>(phrase);
                    });
                    output.Add(phrase.Result);
                }
            });
            return Result<List<PhraseDto>>.Success(output);
        }

        public static List<string> SplitToTermValues(this string text, bool allCaps=true)
        {
            text = text.Normalize().WithoutSquareBrackets();
            //Console.WriteLine($"TEXT IS: {text}");
            var words = text.Split(null).ToList();
            //filter out empty words
            var origLength = words.Count;
            words = words.Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
            if (words.Count != origLength)
            {
                Console.WriteLine($"Removed {origLength - words.Count} empty words");
            }
            if (allCaps)
                words = words.TakeWhile(w => Regex.IsMatch(w, @"[^\s+]")).Select(w => w.ToUpper()).ToList();
            else
                words = words.TakeWhile(w => Regex.IsMatch(w, @"[^\s+]")).ToList();
            return words;
        }

        public static List<string> SplitToNormalizedTerms(this string text)
        {
            var words = text.SplitToTermValues(false);
            return words.Select(v => v.AsTermValue()).ToList();
        }
    }
}