using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Translate
{
    public class GetTranslations
    {
        public class Query : IRequest<Result<List<TranslationResultDto>>>
        {
            public TermDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<TranslationResultDto>>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly ITranslator _translator;
            public Handler(DataContext context, IUserAccessor userAccessor, ITranslator translator)
            {
            this._translator = translator;
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<List<TranslationResultDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GetTranslationsAsync(_translator, request.Dto, _userAccessor.GetUsername());
            }
        }
    }
}