using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.ContentHistory;
using Application.DomainDTOs.UserLanguageProfile;
using Application.Extensions;
using Application.Interfaces;
using Application.Parsing;
using Application.Utilities;
using Domain.DataObjects;
using MediatR;
using Persistence;

namespace Application
{
    public class DataRepository : IDataRepository
    {
        private readonly Func<DataContext> _factory;

        public DataContext context {get {return _factory.Invoke();} }
        public DataRepository(Func<DataContext> factory) 
        {
            this._factory = factory;
        }

        public async Task<Result<Unit>> AddContentTag(ContentTagDto dto)
        {
            // Create a scoped context instance like so
            using (var context = _factory.Invoke())
            {
                return await context.AddContentTag(dto);
            }
        }

        public async Task<Result<ContentHistory>> ContentHistoryFor(string username, string contentUrl)
        {
            using (var context = _factory.Invoke())
            {
                return await context.ContentHistoryFor(username, contentUrl);
            }
        }

        public async Task<Result<Unit>> CreateTerm(string language, string termValue)
        {
            using (var context = _factory.Invoke())
            {
                return await context.CreateTerm(language, termValue);
            }

        }

        public async Task<Result<Unit>> CreateUserTerm(UserTermDto dto, string username)
        {
            using (var context = _factory.Invoke())
            {
                return await context.CreateUserTerm(dto, username);
            }
        }

        public async Task<Result<List<ContentMetadataDto>>> GetContentsForLanguage(string username, string lang)
        {
            using (var context = _factory.Invoke())
            {
                return await context.GetContentsForLanguage(username, lang);
            }
        }

        public async Task<Result<List<ContentMetadataDto>>> GetContentsWithTag(string tagValue)
        {
            using (var context = _factory.Invoke())
            {
                return await context.GetContentsWithTag(tagValue);
            }
        }

        public async Task<Result<List<ContentTagDto>>> GetContentTags(Guid contentId)
        {
            using (var context = _factory.Invoke())
            {
                return await context.GetContentTags(contentId);
            }
         }

        public async Task<Result<KnownWordsDto>> GetKnownWords(Guid contentId, string username, IParserService parser)
        {
            using (var context = _factory.Invoke())
            {
                return await context.GetKnownWords(contentId, username, parser);
            }
 
        }

        public async Task<Result<DailyKnownWordsDto>> GetKnownWordsForDay(DateTime dateTime, string lang, string username)
        {
            using (var context = _factory.Invoke())
            {
                return await context.GetKnownWordsForDay(dateTime, lang, username);
            }
 
        }

        public async Task<Result<DailyKnownWordsDto>> GetKnownWordsForDay(DateTime date, Guid langProfileId)
        {
            using (var context = _factory.Invoke())
            {
                return await context.GetKnownWordsForDay(date, langProfileId);
            }
        }

        public async Task<Result<ContentMetadataDto>> GetMetadataFor(string url)
        {
            using (var context = _factory.Invoke())
            {
                return await context.GetMetadataFor(url);
            }    
        }

        public async Task<Result<ContentMetadataDto>> GetMetadataFor(string username, string url)
        {
           using (var context = _factory.Invoke())
            {
                return await context.GetMetadataFor(username, url);
            }
        }

        public async Task<Result<ContentMetadataDto>> GetMetadataFor(string username, Guid contentId)
        {
           using (var context = _factory.Invoke())
            {
                return await context.GetMetadataFor(username, contentId);
            }
        }

        public async Task<Result<KnownWordsDto>> KnownWordsForSection(ContentSection section, Guid languageProfileId)
        {
            using (var context = _factory.Invoke())
            {
                return await context.KnownWordsForSection(section, languageProfileId);
            }
        }

        public async Task<Result<ContentViewRecord>> LatestRecordFor(string username, string contentUrl)
        {
            using (var context = _factory.Invoke())
            {
                return await context.LatestRecordFor(username, contentUrl);
            }
        }

        public async Task<Result<Unit>> SetLastStudiedLanguage(string iso, string username)
        {
            using (var context = _factory.Invoke())
            {
                return await context.SetLastStudiedLanguage(iso, username);
            }
        }

        public async Task<bool> TermKnown(Guid langProfileId, string term, int threshold)
        {
            using (var context = _factory.Invoke())
            {
                return await context.TermKnown(langProfileId, term, threshold);
            }
        }

        public async Task<Result<Unit>> UpdateUserTerm(UserTermDto dto)
        {
            using (var context = _factory.Invoke())
            {
                return await context.UpdateUserTerm(dto);
            }
        }

        public async Task<Result<List<UserTermDetailsDto>>> UserTermsDueNow(LanguageNameDto dto, string username)
        {
            using (var context = _factory.Invoke())
            {
                return await context.UserTermsDueNow(dto, username);
            }
        }

        public async Task<Result<KnownWordsDto>> KnownWordsForList(List<string> words, Guid languageProfileId)
        {
            using (var context = _factory.Invoke())
            {
                var list = await context.KnownWordsForList(words, languageProfileId);
                if (list == null)
                    return Result<KnownWordsDto>.Failure("Could not retreive list");
                return Result<KnownWordsDto>.Success(list);
            }
        }

        public async Task<Result<UserLanguageProfile>> ProfileFor(string username, string language)
        {
            using (var context = _factory.Invoke())
            {
                return await context.GetProfileAsync(username, language);
            }
        }

        public async Task<Result<SectionAbstractTerms>> AbstractTermsForSection(string contentUrl, int index, string username, IParserService parser)
        {
            using (var context = _factory.Invoke())
            {
                var metadata = await parser.GetContentMetadata(contentUrl);
                var section = await parser.GetSection(contentUrl, index);
                if (section == null)
                    return Result<SectionAbstractTerms>.Failure("Could not get section!");
                var groups = new List<ElementAbstractTerms>();
                foreach(var element in section.TextElements)
                {
                    var words = element.Value.Split(' ').ToList();
                    var terms = new List<AbstractTermDto>();
                    for(int i = 0; i < words.Count; ++i)
                    {
                        var result = await context.AbstractTermFor(new TermDto{Value = words[i], Language = metadata.Language}, username);
                        if (!result.IsSuccess)
                            return Result<SectionAbstractTerms>.Failure($"Failed to get abstract term! Error message: {result.Error}");
                        result.Value.IndexInChunk = i;
                        terms.Add(result.Value);    
                    }
                    var termGroup = new ElementAbstractTerms
                    {
                        Tag = element.Tag,
                        AbstractTerms = terms
                    };
                    groups.Add(termGroup);
                }

                var output = new SectionAbstractTerms
                {
                    ContentUrl = contentUrl,
                    Index = index,
                    SectionHeader = section.SectionHeader,
                    ElementGroups = groups
                };
                return Result<SectionAbstractTerms>.Success(output);
            }
        }

        
    }
}