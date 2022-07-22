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
using Application.Parsing;
using Domain.DataObjects;
using MediatR;

namespace Application.Interfaces
{
    public interface IDataRepository
    {
        public Task<Result<ContentMetadataDto>> GetMetadataFor(string username, Guid contentId);
        public Task<Result<List<ContentMetadataDto>>> GetContentsForLanguage(string username, string lang);
        public Task<Result<AbstractTermDto>> GetAbstractTerm(TermDto term, string username);
        // from ContentHistoryContextEntensions.cs
        public Task<Result<UserLanguageProfile>> ProfileFor(string username, string language);
        public Task<Result<bool>> WordIsKnown(string value, Guid languageProfileId);
    }
}