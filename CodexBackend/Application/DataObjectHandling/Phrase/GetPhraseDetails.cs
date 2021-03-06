using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Phrase;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Phrase
{
    public class GetPhraseDetails
    {
        public class Query : IRequest<Result<PhraseDto>>
        {
            public PhraseQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PhraseDto>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<PhraseDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GetPhraseDetails(request.Dto, _userAccessor.GetUsername());
            }
        }
    }
}