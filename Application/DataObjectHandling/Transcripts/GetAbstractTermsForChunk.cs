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

namespace Application.DataObjectHandling.Transcripts
{
    public class GetAbstractTermsForChunk
    {
        public class Query : IRequest<Result<List<AbstractTermDto>>>
        {
            public GetTranscriptChunkDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<AbstractTermDto>>>
        {
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._context = context;
            this._userAccessor = userAccessor;
            }

            public async Task<Result<List<AbstractTermDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = _userAccessor.GetUsername();
                return await _context.AbstractTermsFor(request.Dto.TranscriptChunkId, username);
            }
        }
    }
}