using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content.Queries;
using Application.Extensions;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class UnsaveContent
    {
        public class Command : IRequest<Result<Unit>>
        {
            public SaveContentQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _context.UnsaveContent(request.Dto.ContentUrl, request.Dto.LanguageProfileId);
            }
        }
    }
}