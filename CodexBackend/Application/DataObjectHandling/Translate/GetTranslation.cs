using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Translator;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Translate
{
    public class GetTranslation
    {
        public class Query : IRequest<Result<TranslatorResponse>>
        {
            public TranslatorQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<TranslatorResponse>>
        {
        private readonly ITranslator _translator;
        private readonly DataContext _context;
            public Handler(ITranslator translator, DataContext context)
            {
            this._context = context;
            this._translator = translator;
            }

            public async Task<Result<TranslatorResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var existing = await _context.GetTopTranslation(request.Dto);
                if (existing.IsSuccess)
                    return existing;
                return await _translator.GetTranslation(request.Dto);
            }
        }
    }
}