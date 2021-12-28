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
using Application.DomainDTOs;
using Application.DomainDTOs.UserTerm;

namespace Application.DataObjectHandling.UserTerms
{
    
    public class UserTermListTranslations
    {
        public class Query : IRequest<Result<List<TranslationDto>>>
        {
            public UserTermIdDto GetTranslationsDto { get; set; }
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
                    dtoList.Add(new TranslationDto
                    {
                        TermValue = t.TermValue,
                        TermLanguage = t.TermLanguage,
                        UserValue = t.UserValue,
                        UserLanguage = t.UserLanguage
                    });
                }
                return Result<List<TranslationDto>>.Success(dtoList);
            }
        }
    }
}