using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Domain.DataObjects;

namespace Application.DataObjectHandling.UserTerms
{
    public class GetTranslationsDto
    {
        public Guid UserTermId { get; set; }
    }
    public class TranslationDto
    {
        //the Translation class w/o UserTerm properties
        public Guid TranslationId { get; set; }
        public string Value { get; set; }

        public static TranslationDto AsDto(Translation t)
        {
            return new TranslationDto
            {
                TranslationId = t.TranslationId,
                Value = t.Value
            };
        }
    }
    public class UserTermListTranslations
    {
        public class Query : IRequest<Result<List<TranslationDto>>>
        {
            public GetTranslationsDto GetTranslationsDto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<TranslationDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<TranslationDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userTerm = await _context.UserTerms
                .Include(t => t.Translations)
                .FirstOrDefaultAsync(x => x.UserTermId == request.GetTranslationsDto.UserTermId);
                
                if(userTerm == null) return Result<List<TranslationDto>>.Failure("User Term not found");
                var list = userTerm.Translations.ToList();
                var dtoList = new List<TranslationDto>();
                foreach(var t in list)
                {
                    dtoList.Add(TranslationDto.AsDto(t));
                }
                return Result<List<TranslationDto>>.Success(dtoList);
            }
        }
    }
}