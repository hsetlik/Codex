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

        public async Task<Result<UserLanguageProfile>> ProfileFor(string username, string language)
        {
            using (var context = _factory.Invoke())
            {
                return await context.GetProfileAsync(username, language);
            }
        }

        
        public async Task<Result<AbstractTermDto>> GetAbstractTerm(TermDto term, string username)
        {
            using (var context = _factory.Invoke())
            {
                return await context.AbstractTermFor(term, username);
            }
        }


        public async Task<Result<bool>> WordIsKnown(string value, Guid languageProfileId)
        {
            using (var context = _factory.Invoke())
            {
                var known = await context.TermKnown(languageProfileId, value);
                return Result<bool>.Success(known);
            }
        }
    }
}