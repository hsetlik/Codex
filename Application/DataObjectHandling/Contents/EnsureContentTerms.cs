using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Extensions;
using Persistence;
using System.Text.RegularExpressions;
using Application.DomainDTOs.Content;

namespace Application.DataObjectHandling.Contents
{
    public class EnsureContentTerms
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ContentIdDto Dto { get; set; }
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
               return await _context.EnsureTermsForContent(request.Dto.ContentId);
            }
        }

    }
}