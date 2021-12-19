using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataObjects;

namespace Application.DomainDTOs
{
    public class TranslationDto
    {
        //the Translation class w/o UserTerm properties
        public Guid TranslationId { get; set; }
        public string Value { get; set; }

        public static TranslationDto AsDto(UserTermTranslation t)
        {
            return new TranslationDto
            {
                TranslationId = t.TranslationId,
                Value = t.Value
            };
        }
    }
}