using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Translator;
using Application.Interfaces;
using MediatR;

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
            public Handler(ITranslator translator)
            {
            this._translator = translator;
            }

            public async Task<Result<TranslatorResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                Console.WriteLine($"Requesting translation for {request.Dto.QueryValue} in language {request.Dto.QueryLanguage}");
                return await _translator.GetTranslation(request.Dto);
            }
        }
    }
}