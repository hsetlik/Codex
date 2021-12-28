using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;
using AutoMapper;
using Domain;
using Domain.DataObjects;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ContentMetadataDto, Content>();
            
           

            

        }
    }
}