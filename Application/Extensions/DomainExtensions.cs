using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.Utilities;
using Domain.DataObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Extensions
{
    public static class DomainExtensions
    {
       public static void UpdateTranslations(this UserTerm userTerm, List<string> values)
        {
            foreach (var t in values)
            {
                bool exists = false;
                foreach (var translation in userTerm.Translations)
                {
                   if (t == translation.Value)
                   {
                       exists = true;
                       break;
                   }
                }
                if (!exists)
                {
                    var translation = new UserTermTranslation
                    {
                        Value = t,
                        UserTerm = userTerm
                    };
                    userTerm.Translations.Add(translation);
                }
            }
        }
        
        public static List<UserTermTranslation> GetAsTranslations(this UserTerm userTerm, List<string> values)
        {
            var output = new List<UserTermTranslation>();
            foreach(var value in values)
            {
                var translation = new UserTermTranslation
                {
                    Value = value,
                    UserTerm = userTerm
                };
                output.Add(translation);
            }
            return output;
        }

        public static List<string> GetTranslationStrings(this UserTerm userTerm)
        {
            var output = new List<string>();
            foreach(var t in userTerm.Translations)
            {
                output.Add(t.Value);
            }
            return output;
        }

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
            input.DateTimeDue = nextDueDate.ToString();
            return input;
        }

        // just save some typing
        public static UserTermDetailsDto GetUserTermDetailsDto(this UserTerm userTerm)
        {
             return new UserTermDetailsDto
            {
                TermValue = userTerm.Term.NormalizedValue,
                TimesSeen = userTerm.TimesSeen,
                EaseFactor = userTerm.EaseFactor,
                Rating = userTerm.Rating,
                DateTimeDue = userTerm.DateTimeDue,
                SrsIntervalDays = userTerm.SrsIntervalDays,
                UserTermId = userTerm.UserTermId,
                CreatedAt = userTerm.CreatedAt
            };
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
            return userTerm;
        }

        public static ContentHistory AppendRecord(this ContentHistory history, ContentViewRecord record)
        {
            history.ContentViewRecords.Add(record);
            return history;
        }

        public static void AppendRecord(this UserLanguageProfile profile, ContentViewRecord record)
        {
            //TODO
            //profile.ContentHistory.ContentViewRecords.Add(record);
        }

        public static UserTermDto GetDto(this UserTerm term)
        {
            return new UserTermDto
            {
                Value = term.Term.NormalizedValue,
                Language = term.Term.Language,
                EaseFactor = term.EaseFactor,
                SrsIntervalDays = term.SrsIntervalDays,
                Rating = term.Rating,
                TimesSeen = term.TimesSeen,
                UserTermId = term.UserTermId,
                Translations = term.GetTranslationStrings()
            };
            
        }


        public static ContentMetadataDto GetMetadata(this Content content)
        {
            return new ContentMetadataDto
            {
                ContentUrl = content.ContentUrl,
                ContentType = content.ContentType,
                ContentName = content.ContentName,
                Language = content.Language,
                AudioUrl = content.AudioUrl,
                VideoUrl = content.VideoUrl,
                ContentId = content.ContentId,
                NumSections = content.NumSections
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
    }
}