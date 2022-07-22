using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.Utilities;
using Domain.DataObjects;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class DtoExtensions
    {
        public static UserTermDto AsUserTerm(this AbstractTermDto dto)
        {
            var uTerm = new UserTermDto
            {
                Value = dto.TermValue,
                Language = dto.Language,
                EaseFactor = dto.EaseFactor,
                SrsIntervalDays = dto.SrsIntervalDays,
                Rating = dto.Rating,
                Translations = dto.Translations,
                TimesSeen = dto.TimesSeen
            };
            return uTerm;
        }
    }
}