using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.Interfaces;
using Application.Parsing;
using Application.Utilities;
using Domain.DataObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Extensions
{
    public static class DomainExtensions
    {

        // works something like this: https://en.wikipedia.org/wiki/SuperMemo
        public static UserTerm AnsweredWith(this UserTerm input, int answer)
        {
            input.Rating = answer;
            if (answer > 2)
            {
                if (input.TimesSeen == 0)
                    input.SrsIntervalDays = 0.125f; // 1/8 days (3 hrs)
                else if(input.TimesSeen == 1)
                    input.SrsIntervalDays = 3.0f; //this was six in the original SM-2 algorithm
                else
                    input.SrsIntervalDays = input.SrsIntervalDays * input.EaseFactor;
                input.TimesSeen += 1;
            }
            else
            {
                input.TimesSeen = 0;
                input.SrsIntervalDays = 0.0125f; // 0.0125 days = 18 minutes
            }
            //update ease factor - coefficients can be tweaked
            input.EaseFactor = input.EaseFactor + (0.1f - (5 - answer) * (0.08f + (5 - answer) * 0.02f));
            const float minimumEase = 1.3f;
            if (input.EaseFactor < minimumEase)
                input.EaseFactor = minimumEase;
            //new due date
            var nextDueDate = DateTime.Now.AddDays((double)input.SrsIntervalDays);
            input.DateTimeDue = nextDueDate;
            return input;
        }

        public static UserTerm UpdatedWith(this UserTerm userTerm, UserTermDetailsDto details)
        {
            userTerm.TimesSeen = details.TimesSeen;
            userTerm.EaseFactor = details.EaseFactor;
            userTerm.Rating = details.Rating;
            userTerm.DateTimeDue = details.DateTimeDue;
            userTerm.SrsIntervalDays = details.SrsIntervalDays;
            userTerm.UserTermId = details.UserTermId;
            userTerm.CreatedAt = details.CreatedAt;
            userTerm.Starred = details.Starred;
            return userTerm;
        }

        public static ContentHistory AppendRecord(this ContentHistory history, ContentViewRecord record)
        {
            history.ContentViewRecords.Add(record);
            return history;
        }

        public static UserTermDto GetDto(this UserTerm term)
        {
            return new UserTermDto
            {
                Value = term.NormalizedTermValue,
                Language = term.Language,
                EaseFactor = term.EaseFactor,
                SrsIntervalDays = term.SrsIntervalDays,
                Rating = term.Rating,
                TimesSeen = term.TimesSeen,
                UserTermId = term.UserTermId,
                Translations = term.Translations.Select(t => t.UserValue).ToList()
            };
        }

        public static List<TermDto> TermList(this Phrase phrase)
        {
            var words = phrase.Value.Split(null).ToArray();
            var output = new List<TermDto>();
            foreach(var word in words)
            {
                output.Add(new TermDto
                {
                    Value = word.AsTermValue(),
                    Language = phrase.UserLanguageProfile.Language
                });
            }
            return output;
        }

        public static Phrase UpdatedWith(this Phrase input, PhraseDetailsDto details)
        {
            input.Value = details.Value;
            input.TimesSeen = details.TimesSeen;
            input.EaseFactor = details.EaseFactor;
            input.Rating = details.Rating;
            input.DateTimeDue = details.DateTimeDue;
            input.SrsIntervalDays = details.SrsIntervalDays;

            if (input.Translations.Count > details.Translations.Count)
            { //deleting a translation
                foreach(var translation in input.Translations)
                {
                    if (!details.Translations.Contains(translation.Value))
                        input.Translations.Remove(translation);
                }
            }
            else if (input.Translations.Count < details.Translations.Count)
            { // adding a translation
                foreach(var word in details.Translations)
                {
                    if (input.Translations.Any(p => p.Value == word))
                    {
                        var translation = new PhraseTranslation
                        {
                            Value = word,
                            PhraseId = input.PhraseId,
                            Phrase = input,
                            TranslationId = Guid.NewGuid()
                        };
                        input.Translations.Add(translation);
                    }
                }
            }
            return input;
        }

        public static async Task<List<UserTermCreateQuery>> CreatorsFor(this ContentSection section, ITranslator translator, string language)
        {
            //Console.WriteLine($"Getting creators for {section.ContentUrl} section #{section.Index}");
            var output = new List<UserTermCreateQuery>();
            var words = section.Body.Split(null).ToList();
            foreach(var word in words)
            {
                var tResult = await translator.GetTranslation(new DomainDTOs.Translator.TranslatorQuery
                {
                    ResponseLanguage = (language == "en") ? "de" : "en",
                    QueryLanguage = language,
                    QueryValue = word
                });
                if (tResult.IsSuccess)
                {
                    output.Add(new UserTermCreateQuery
                    {
                        TermValue = word,
                        Language = language,
                        FirstTranslation = tResult.Value.ResponseValue
                    });
                    //Console.WriteLine($"Word is: {word} with translation {tResult.Value.ResponseValue}");
                }
                else
                {
                    //Console.WriteLine($"Translation failed! Error message: {tResult.Error}");
                }
            }
            return output;
        }

        public static DailyProfileHistory CreateHistory(this UserLanguageProfile profile)
        {
            return new DailyProfileHistory
            {
                UserLanguageProfile = profile,
                LanguageProfileId = profile.LanguageProfileId
            };
        }

        public static UserLanguageProfile CreateProfileFor(this CodexUser user, string language)
        {
            var prof =  new UserLanguageProfile
            {
                Language = language,
                User = user,
                UserId = user.Id,
                KnownWords = 0,
                UserLanguage = user.NativeLanguage
            };
            prof.DailyProfileHistory = prof.CreateHistory();
            return prof;
        }
    }
}