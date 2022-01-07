using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Phrase;
using Application.DomainDTOs.Phrase.Responses;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Phrase
{
    public class GetAbstractPhrase
    {
        public class Query : IRequest<Result<AbstractPhraseDto>>
        {
            public PhraseQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<AbstractPhraseDto>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly ITranslator _translator;
            public Handler(DataContext context, ITranslator translator, IUserAccessor userAccessor)
            {
            this._translator = translator;
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<AbstractPhraseDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GetAbstractPhrase(request.Dto, _translator, _userAccessor);
            }
        }
    }
}