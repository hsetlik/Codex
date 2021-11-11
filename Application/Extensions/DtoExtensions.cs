using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;

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
            };
            return uTerm;
        }
        
    }
}