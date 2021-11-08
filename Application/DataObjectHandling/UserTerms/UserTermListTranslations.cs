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
    public class UserTermListTranslations
    {
        public class Query : IRequest<Result<List<Translation>>>
        {
            public GetTranslationsDto GetTranslationsDto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<Translation>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<Translation>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userTerm = await _context.UserTerms
                .Include(t => t.Translations)
                .FirstOrDefaultAsync(x => x.UserTermId == request.GetTranslationsDto.UserTermId);

                if(userTerm == null) return Result<List<Translation>>.Failure("User Term not found");
                var list = userTerm.Translations.ToList();
                return Result<List<Translation>>.Success(list);
            }
        }
    }
}