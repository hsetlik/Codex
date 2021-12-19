using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Extensions;
using Application.Utilities;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Terms
{
    public class PopularTranslationsList
    {
        public class Query : IRequest<Result<List<TranslationResultDto>>>
        {
            public TermDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<TranslationResultDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Result<List<TranslationResultDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.ListPopularTranslations(request.Dto);
            }
        }

    }
}