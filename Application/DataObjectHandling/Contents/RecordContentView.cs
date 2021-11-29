using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Extensions;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class RecordContentView
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid ContentId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
            this._context = context;
            this._userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _context.RecordContentView(request.ContentId, _userAccessor.GetUsername());
            }
        }
    }
}