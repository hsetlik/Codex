using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs.UserLanguageProfile;
using Application.Interfaces;
using Application.Parsing;
using Application.Utilities;
using AutoMapper;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class KnownWordsContextExtensions
    {
        //KNOWN WORDS
        public static async Task<Result<KnownWordsDto>> GetKnownWords(this DataContext context, Guid contentId, string username, IParserService parser)
        {

            int total = 0;
            int known = 0;
            var metadata = await context.GetMetadataFor(username, contentId);
            if (!metadata.IsSuccess)
                return Result<KnownWordsDto>.Failure($"Could not load metadata! Error message: {metadata.Error}");
            var profile = await context.UserLanguageProfiles
            .Include(u => u.User)
            .FirstOrDefaultAsync(p => p.Language == metadata.Value.Language && p.User.UserName == username);
            if (profile == null)
                return Result<KnownWordsDto>.Failure("Could not load profile");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var sections = await parser.GetAllSections(metadata.Value.ContentUrl);
            watch.Stop();
            watch.Restart();
            //var tasks = new List<Task<Result<KnownWordsDto>>>();
            foreach (var section in sections)
            {
                //tasks.Add(context.KnownWordsForSection(section, profile.LanguageProfileId));
                var r = await context.KnownWordsForSection(section, profile.LanguageProfileId);
                if (r.IsSuccess)
                {
                    known += r.Value.KnownWords;
                    total += r.Value.TotalWords;
                }
            }
            watch.Stop();
            float perTerm = (float)watch.ElapsedMilliseconds / (float)total;
            //Line($"Checked {total} terms in {watch.ElapsedMilliseconds} ms ({perTerm} ms/term on average)");
            return Result<KnownWordsDto>.Success(new KnownWordsDto
            {
                KnownWords = known,
                TotalWords = total
            });
        }

        public static async Task<Result<KnownWordsDto>> KnownWordsForSection(this DataContext context, ContentSection section, Guid languageProfileId)
        {
            var terms = section.Body.Split(null);
            int known = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var term in terms)
            {
                if (await context.TermKnown(languageProfileId, term))
                    known += 1;
            }
            watch.Stop();

            return Result<KnownWordsDto>.Success(new KnownWordsDto
            {
                TotalWords = terms.Length,
                KnownWords = known
            });
        }


        public static async Task<KnownWordsDto> KnownWordsForList(this DataContext context, List<string> words, Guid languageProfileId)
        {
            int known = 0;
            var tasks = new List<Task<bool>>();
            foreach (var word in words)
            {
                tasks.Add(context.TermKnown(languageProfileId, word));
            }
            var data = await Task.WhenAll(tasks);
            foreach (var word in data)
            {
                if (word)
                    ++known;
            }
            return new KnownWordsDto
            {
                KnownWords = known,
                TotalWords = words.Count
            };
        }

        public static async Task<Result<KnownWordsDto>> KnownWordsForListAsync(this DataContext context, List<string> words, Guid languageProfileId)
        {
            int known = 0;
            var tasks = new List<Task<bool>>();
            bool[] data = { };
            try
            {
                foreach (var word in words)
                {
                    tasks.Add(context.TermKnown(languageProfileId, word));
                }
                data = await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                return Result<KnownWordsDto>.Failure($"Failed with exception: {ex.Message}");
            }
            foreach (var word in data)
            {
                if (word)
                    ++known;
            }
            var dto = new KnownWordsDto
            {
                KnownWords = known,
                TotalWords = words.Count
            };
            return Result<KnownWordsDto>.Success(dto);
        }


        public static async Task<bool> TermKnown(this DataContext context, Guid languageProfileId, string term, int threshold = 3)
        {
            var userTerm = await context.UserTerms.FirstOrDefaultAsync(u => u.LanguageProfileId == languageProfileId && u.NormalizedTermValue == term.ToUpper());
            if (userTerm != null && userTerm.Rating >= threshold)
                return true;
            return false;
        }

    }
}