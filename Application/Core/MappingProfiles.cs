using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.UserLanguageProfile;
using Application.DomainDTOs.UserTerm;
using Application.Parsing;
using AutoMapper;
using Domain;
using Domain.DataObjects;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Content, ContentMetadataDto>();
            CreateMap<UserTerm, UserTermDetailsDto>();
            CreateMap<Translation, TranslationDto>();
            CreateMap<ContentSection, ContentSectionDto>();
            CreateMap<UserLanguageProfile, LanguageProfileDto>();
            CreateMap<UserTerm, AbstractTermDto>();
        }
    }

    public static class MapperFactory
    {
        public static IMapper GetDefaultMapper()
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<MappingProfiles>();
            });
            return new Mapper(config);
        }
    }
}