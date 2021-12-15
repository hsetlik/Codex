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
using Application.Parsing;
using Domain.DataObjects;
using MediatR;

namespace Application.Interfaces
{
    public interface IDataRepository
    {
        // from ContentContextExtensions.cs
        public Task<Result<Unit>> AddContentTag(ContentTagDto dto);
        public Task<Result<List<ContentTagDto>>> GetContentTags(Guid contentId);
        public Task<Result<List<ContentMetadataDto>>> GetContentsWithTag(string tagValue);
        public Task<Result<ContentMetadataDto>> GetMetadataFor(string url);
        public Task<Result<ContentMetadataDto>> GetMetadataFor(string username, string url);
        public Task<Result<ContentMetadataDto>> GetMetadataFor(string username, Guid contentId);
        public Task<Result<List<ContentMetadataDto>>> GetContentsForLanguage(string username, string lang);
        public Task<Result<SectionAbstractTerms>> AbstractTermsForSection(string contentUrl, int index, string username, IParserService parser);
        // from ContentHistoryContextEntensions.cs
        public Task<Result<ContentHistory>> ContentHistoryFor(string username, string contentUrl);
        public Task<Result<ContentViewRecord>> LatestRecordFor(string username, string contentUrl);
        // from DailyProfileHistoryExtensions.cs
        public Task<Result<DailyKnownWordsDto>> GetKnownWordsForDay(DateTime dateTime, string lang, string username);
        public Task<Result<DailyKnownWordsDto>> GetKnownWordsForDay(DateTime date, Guid langProfileId);
        //from KnownWordsContextExtensions.cs
        public Task<Result<KnownWordsDto>> GetKnownWords(Guid contentId, string username, IParserService parser);
        public Task<Result<KnownWordsDto>> KnownWordsForSection(ContentSection section, Guid languageProfileId);
        public Task<Result<KnownWordsDto>> KnownWordsForList(List<string> words, Guid languageProfileId);
        public Task<bool> TermKnown(Guid langProfileId, string term, int threshold);
        //from TermContextExtensions.cs
        public Task<Result<Unit>> CreateTerm(string language, string termValue);
        // from UserContextExtensions
        public Task<Result<Unit>> SetLastStudiedLanguage(string iso, string username);
        // from UserTermContextExtensions.cs
        public Task<Result<Unit>> UpdateUserTerm(UserTermDto dto);
        public Task<Result<Unit>> CreateUserTerm(UserTermDto dto, string username);
        public Task<Result<List<UserTermDetailsDto>>> UserTermsDueNow(LanguageNameDto dto, string username);

        //other getters
        public Task<Result<UserLanguageProfile>> ProfileFor(string username, string language);
    }
}