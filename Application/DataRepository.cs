using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
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
        public async Task<Result<List<ContentMetadataDto>>> GetContentsForLanguage(string username, string lang)
        {
            using (var context = _factory.Invoke())
            {
                return await context.GetContentsForLanguage(username, lang);
            }
        }


        public async Task<Result<ContentMetadataDto>> GetMetadataFor(string username, Guid contentId)
        {
           using (var context = _factory.Invoke())
            {
                return await context.GetMetadataFor(username, contentId);
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
                    var words = element.Value.Split(null).ToList();
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
                        AbstractTerms = terms,
                        Index = groups.Count
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
        public async Task<Result<AbstractTermDto>> GetAbstractTerm(TermDto term, string username)
        {
            using (var context = _factory.Invoke())
            {
                return await context.AbstractTermFor(term, username);
            }
        }

        Task<Result<DomainDTOs.UserLanguageProfile.KnownWordsDto>> IDataRepository.KnownWordsForList(List<string> words, Guid languageProfileId)
        {
            throw new NotImplementedException();
        }
    }
}